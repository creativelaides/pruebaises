using System.Diagnostics;
using Microsoft.Extensions.Options;
using TarifasElectricas.Application.Contracts.Persistence;
using TarifasElectricas.Application.Contracts.Services;
using TarifasElectricas.Domain.Entities;
using TarifasElectricas.Domain.Enums;
using TarifasElectricas.Domain.ValueObjects;
using TarifasElectricas.Infrastructure.Options;
using TarifasElectricas.Infrastructure.Utils.Mapping;

namespace TarifasElectricas.Infrastructure.Services;

/// <summary>
/// Servicio ETL que extrae datos de Socrata, los transforma y los guarda en la BD.
/// </summary>
public sealed class SocrataEtlService : IEtlService
{
    private readonly SocrataClient _client;
    private readonly IUnitOfWork _unitOfWork;
    private readonly SocrataOptions _options;

    public SocrataEtlService(
        SocrataClient client,
        IUnitOfWork unitOfWork,
        IOptions<SocrataOptions> options)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
    }

    /// <summary>
    /// Ejecuta el ETL completo: lectura paginada, upsert de empresas y tarifas,
    /// y registro de ejecución.
    /// </summary>
    public async Task<EtlExecutionResult> ExecuteAsync()
    {
        var executionDate = DateTime.UtcNow;
        var stopwatch = Stopwatch.StartNew();

        var processed = 0;
        var errors = 0;
        var created = 0;
        var updated = 0;

        try
        {
            var companies = (await _unitOfWork.Companies.GetAllCompaniesAsync())
                .ToDictionary(c => c.Code, StringComparer.OrdinalIgnoreCase);

            var tariffsByYear = new Dictionary<int, Dictionary<TariffKey, ElectricityTariff>>();

            var offset = 0;
            var limit = Math.Max(1, _options.PageSize);

            while (true)
            {
                var rows = await _client.GetPageAsync(offset, limit, CancellationToken.None);
                if (rows.Count == 0)
                    break;

                foreach (var row in rows)
                {
                    if (!SocrataRowMapper.TryMapRow(row, _options.Fields, out var mapped))
                    {
                        errors++;
                        continue;
                    }

                    if (!companies.TryGetValue(mapped.TariffOperator, out var company))
                    {
                        company = new Company(mapped.TariffOperator);
                        await _unitOfWork.Companies.AddAsync(company);
                        companies[mapped.TariffOperator] = company;
                    }

                    if (!tariffsByYear.TryGetValue(mapped.Year, out var existingByKey))
                    {
                        var existing = await _unitOfWork.ElectricityTariffs.GetByYearAsync(mapped.Year);
                        existingByKey = existing.ToDictionary(
                            t => TariffKey.From(t.Period),
                            TariffKeyComparer.Instance);
                        tariffsByYear[mapped.Year] = existingByKey;
                    }

                    var key = TariffKey.From(mapped);
                    var costs = new TariffCosts(
                        mapped.TotalCu,
                        mapped.PurchaseCostG,
                        mapped.ChargeTransportStnTm,
                        mapped.ChargeTransportSdlDm,
                        mapped.MarketingMargin,
                        mapped.CostLossesPr,
                        mapped.RestrictionsRm,
                        mapped.Cot,
                        mapped.CfmjGfact);

                    if (existingByKey.TryGetValue(key, out var existingTariff))
                    {
                        if (!existingTariff.Costs.Equals(costs))
                        {
                            existingTariff.UpdateCosts(costs);
                            await _unitOfWork.ElectricityTariffs.UpdateAsync(existingTariff);
                            updated++;
                        }

                        processed++;
                        continue;
                    }

                    var period = new TariffPeriod(
                        mapped.Year,
                        mapped.Period,
                        mapped.Level,
                        mapped.TariffOperator,
                        executionDate.Year);

                    var tariff = new ElectricityTariff(period, costs, company.Id);
                    await _unitOfWork.ElectricityTariffs.AddAsync(tariff);
                    existingByKey[key] = tariff;

                    created++;
                    processed++;
                }

                await _unitOfWork.SaveChangesAsync();

                if (rows.Count < limit)
                    break;

                offset += limit;
            }

            stopwatch.Stop();

            var result = new EtlExecutionResult
            {
                Success = true,
                ProcessedCount = processed,
                ErrorCount = errors,
                Message = $"ETL completado. Creadas: {created}, Actualizadas: {updated}, Errores: {errors}.",
                DurationSeconds = (decimal)stopwatch.Elapsed.TotalSeconds,
                ExecutionDate = executionDate
            };

            await LogExecutionAsync(result, EtlState.Success);

            return result;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();

            var result = new EtlExecutionResult
            {
                Success = false,
                ProcessedCount = processed,
                ErrorCount = errors + 1,
                Message = $"ETL fallido: {ex.Message}",
                DurationSeconds = (decimal)stopwatch.Elapsed.TotalSeconds,
                ExecutionDate = executionDate
            };

            await LogExecutionAsync(result, EtlState.Failed);

            return result;
        }
    }

    /// <summary>
    /// Persiste un registro de ejecución ETL con métricas básicas.
    /// </summary>
    private async Task LogExecutionAsync(EtlExecutionResult result, EtlState state)
    {
        var log = new EtlLog(
            result.ExecutionDate,
            state,
            result.ProcessedCount,
            result.Message,
            result.DurationSeconds);

        await _unitOfWork.EtlLogs.AddAsync(log);
        await _unitOfWork.SaveChangesAsync();
    }

}

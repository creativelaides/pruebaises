using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TarifasElectricas.Domain.Entities;
using TarifasElectricas.Domain.ValueObjects;

namespace TarifasElectricas.Persistence.Configurations;

public class ElectricityTariffConfiguration : IEntityTypeConfiguration<ElectricityTariff>
{
    public void Configure(EntityTypeBuilder<ElectricityTariff> builder)
    {
        builder.ToTable("electricity_tariffs");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.CompanyId).IsRequired();
        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.DateUpdated).IsRequired();
        builder.Property(x => x.CreatedBy).HasMaxLength(200);
        builder.Property(x => x.UpdatedBy).HasMaxLength(200);

        builder.HasIndex(x => x.CompanyId);

        var periodConverter = new ValueConverter<TariffPeriod, string>(
            v => JsonSerializer.Serialize(new TariffPeriodDto(
                v.Year,
                v.Period,
                v.Level,
                v.TariffOperator), (JsonSerializerOptions?)null),
            v => DeserializePeriod(v));

        var periodComparer = new ValueComparer<TariffPeriod>(
            (l, r) => l!.Equals(r),
            v => v.GetHashCode(),
            v => new TariffPeriod(v.Year, v.Period, v.Level, v.TariffOperator, v.Year));

        builder.Property(x => x.Period)
            .HasConversion(periodConverter)
            .HasColumnType("jsonb")
            .Metadata.SetValueComparer(periodComparer);

        var costsConverter = new ValueConverter<TariffCosts, string>(
            v => JsonSerializer.Serialize(new TariffCostsDto(
                v.TotalCu,
                v.PurchaseCostG,
                v.ChargeTransportStnTm,
                v.ChargeTransportSdlDm,
                v.MarketingMargin,
                v.CostLossesPr,
                v.RestrictionsRm,
                v.Cot,
                v.CfmjGfact), (JsonSerializerOptions?)null),
            v => DeserializeCosts(v));

        var costsComparer = new ValueComparer<TariffCosts>(
            (l, r) => l!.Equals(r),
            v => v.GetHashCode(),
            v => new TariffCosts(
                v.TotalCu,
                v.PurchaseCostG,
                v.ChargeTransportStnTm,
                v.ChargeTransportSdlDm,
                v.MarketingMargin,
                v.CostLossesPr,
                v.RestrictionsRm,
                v.Cot,
                v.CfmjGfact));

        builder.Property(x => x.Costs)
            .HasConversion(costsConverter)
            .HasColumnType("jsonb")
            .Metadata.SetValueComparer(costsComparer);
    }

    private sealed record TariffPeriodDto(
        int Year,
        string Period,
        string Level,
        string TariffOperator);

    private sealed record TariffCostsDto(
        decimal? TotalCu,
        decimal? PurchaseCostG,
        decimal? ChargeTransportStnTm,
        decimal? ChargeTransportSdlDm,
        decimal? MarketingMargin,
        decimal? CostLossesPr,
        decimal? RestrictionsRm,
        decimal? Cot,
        decimal? CfmjGfact);

    private static TariffPeriod DeserializePeriod(string json)
    {
        var dto = JsonSerializer.Deserialize<TariffPeriodDto>(json)
            ?? throw new InvalidOperationException("No se pudo deserializar TariffPeriod.");
        return new TariffPeriod(dto.Year, dto.Period, dto.Level, dto.TariffOperator, dto.Year);
    }

    private static TariffCosts DeserializeCosts(string json)
    {
        var dto = JsonSerializer.Deserialize<TariffCostsDto>(json)
            ?? throw new InvalidOperationException("No se pudo deserializar TariffCosts.");
        return new TariffCosts(
            dto.TotalCu,
            dto.PurchaseCostG,
            dto.ChargeTransportStnTm,
            dto.ChargeTransportSdlDm,
            dto.MarketingMargin,
            dto.CostLossesPr,
            dto.RestrictionsRm,
            dto.Cot,
            dto.CfmjGfact);
    }
}

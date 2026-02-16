using System.Text.Json;
using Microsoft.Extensions.Options;
using TarifasElectricas.Infrastructure.Options;

namespace TarifasElectricas.Infrastructure.Services;

/// <summary>
/// Cliente HTTP mínimo para obtener datos paginados desde Socrata.
/// </summary>
public sealed class SocrataClient
{
    private readonly HttpClient _httpClient;
    private readonly SocrataOptions _options;

    public SocrataClient(HttpClient httpClient, IOptions<SocrataOptions> options)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
    }

    /// <summary>
    /// Obtiene una página de filas del dataset usando offset y límite.
    /// </summary>
    public async Task<IReadOnlyList<JsonElement>> GetPageAsync(int offset, int limit, CancellationToken cancellationToken)
    {
        var requestUri = $"/resource/{_options.DatasetId}.json?$limit={limit}&$offset={offset}";

        using var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
        if (!string.IsNullOrWhiteSpace(_options.AppToken))
            request.Headers.Add("X-App-Token", _options.AppToken);

        using var response = await _httpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();

        await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
        using var document = await JsonDocument.ParseAsync(stream, cancellationToken: cancellationToken);

        if (document.RootElement.ValueKind != JsonValueKind.Array)
            return Array.Empty<JsonElement>();

        var rows = new List<JsonElement>();
        foreach (var element in document.RootElement.EnumerateArray())
            rows.Add(element.Clone());

        return rows;
    }
}

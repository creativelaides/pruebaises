using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using TarifasElectricas.Application.DTOs;

namespace TarifasElectricas.Infrastructure.ExternalServices
{
    public class GovCoApiClient
    {
        private readonly HttpClient _httpClient;
        private const string BASE_URL = "https://www.datos.gov.co/resource/";
        private const string DATASET_ID = "xxxx-xxxx"; // TODO: Replace with actual dataset ID

        public GovCoApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<GovCoTarifaDTO>> ObtenerTarifasAsync()
        {
            var url = $"{BASE_URL}{DATASET_ID}.json";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            
            if (string.IsNullOrEmpty(content))
            {
                return new List<GovCoTarifaDTO>();
            }

            // Configure JsonSerializerOptions to be case-insensitive
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<List<GovCoTarifaDTO>>(content, options) ?? new List<GovCoTarifaDTO>();
        }
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;
using TarifasElectricas.Application.DTOs;

namespace TarifasElectricas.Application.Interfaces
{
    public interface IEtlService
    {
        Task<EtlResult> EjecutarEtlAsync();
        Task<IEnumerable<EtlLogDTO>> GetEtlLogsAsync();
        Task<EtlLogDTO> GetLatestEtlStatusAsync();
    }

    public class EtlResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int RecordsProcessed { get; set; }
    }
}

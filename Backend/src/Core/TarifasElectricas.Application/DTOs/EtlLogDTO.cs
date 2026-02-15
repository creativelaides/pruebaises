using System;

namespace TarifasElectricas.Application.DTOs
{
    public class EtlLogDTO
    {
        public int Id { get; set; }
        public DateTime FechaEjecucion { get; set; }
        public string Estado { get; set; } = string.Empty;
        public int? RegistrosProcesados { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public decimal? DuracionSegundos { get; set; }
    }
}

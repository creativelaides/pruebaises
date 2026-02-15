namespace TarifasElectricas.Application.DTOs
{
    public class TarifaDTO
    {
        public int Id { get; set; }
        public int Año { get; set; }
        public int Mes { get; set; }
        public string Periodo { get; set; } = string.Empty;
        public string Nivel { get; set; } = string.Empty;
        public string Operador { get; set; } = string.Empty;
        public decimal CuTotal { get; set; }
        public decimal? CostoCompraG { get; set; }
        public decimal? CargoTransporteStnTm { get; set; }
        public decimal? CargoTransporteSdlDm { get; set; }
        public decimal? MargenComercializacion { get; set; }
        public decimal? CostoPerdidasPr { get; set; }
        public decimal? RestriccionesRm { get; set; }
        public decimal? Cot { get; set; }
        public decimal? CfmjGfact { get; set; }
    }
}

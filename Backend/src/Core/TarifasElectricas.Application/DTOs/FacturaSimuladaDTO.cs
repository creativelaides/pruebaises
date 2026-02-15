namespace TarifasElectricas.Application.DTOs
{
    public class FacturaSimuladaDTO
    {
        public int ConsumoKwh { get; set; }
        public decimal ValorTotalPagar { get; set; }
        public decimal ValorConsumo { get; set; }
        public decimal ValorCargoFijo { get; set; } // Assuming a fixed charge might exist, or can be 0
        public decimal ValorSubsidioContribucion { get; set; } // Can be positive for subsidy, negative for contribution

        // Breakdown by components
        public decimal CuTotalCalculado { get; set; }
        public decimal? CostoCompraGCalculado { get; set; }
        public decimal? CargoTransporteStnTmCalculado { get; set; }
        public decimal? CargoTransporteSdlDmCalculado { get; set; }
        public decimal? MargenComercializacionCalculado { get; set; }
        public decimal? CostoPerdidasPrCalculado { get; set; }
        public decimal? RestriccionesRmCalculado { get; set; }
        public decimal? CotCalculado { get; set; }
        public decimal? CfmjGfactCalculado { get; set; }
    }
}

namespace TarifasElectricas.Application.DTOs
{
    public class GovCoTarifaDTO
    {
        // These properties will match the field names from the datos.gov.co API response
        // Assuming all fields are strings initially, and will be parsed during transformation
        public string id_tarifa { get; set; } = string.Empty;
        public string anio { get; set; } = string.Empty;
        public string mes { get; set; } = string.Empty;
        public string periodo { get; set; } = string.Empty;
        public string operador { get; set; } = string.Empty;
        public string nivel { get; set; } = string.Empty;
        public string cu_total { get; set; } = string.Empty;
        public string costo_compra_g { get; set; } = string.Empty;
        public string cargo_transporte_stn_tm { get; set; } = string.Empty;
        public string cargo_transporte_sdl_dm { get; set; } = string.Empty;
        public string margen_comercializacion { get; set; } = string.Empty;
        public string costo_perdidas_pr { get; set; } = string.Empty;
        public string restricciones_rm { get; set; } = string.Empty;
        public string cot { get; set; } = string.Empty;
        public string cfmj_gfact { get; set; } = string.Empty;
        public string fecha_actualizacion { get; set; } = string.Empty;
        public string fecha_creacion { get; set; } = string.Empty;
    }
}

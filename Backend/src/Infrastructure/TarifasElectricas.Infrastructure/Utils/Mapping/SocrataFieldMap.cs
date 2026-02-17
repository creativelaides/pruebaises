namespace TarifasElectricas.Infrastructure.Utils.Mapping;

/// <summary>
/// Mapa de columnas del dataset para desacoplar nombres externos del dominio.
/// </summary>
public sealed class SocrataFieldMap
{
    public string Year { get; set; } = "a_o";
    public string Period { get; set; } = "periodo";
    public string Level { get; set; } = "nivel";
    public string TariffOperator { get; set; } = "operador_de_red";

    public string TotalCu { get; set; } = "cu_total";
    public string PurchaseCostG { get; set; } = "costo_compra_gm_i";
    public string ChargeTransportStnTm { get; set; } = "cargo_transporte_stn_tm";
    public string ChargeTransportSdlDm { get; set; } = "cargo_transporte_sdl_dn_m";
    public string MarketingMargin { get; set; } = "margen_comercializaci_n_cvm";
    public string CostLossesPr { get; set; } = "costo_g_t_p_rdidas_prn_m";
    public string RestrictionsRm { get; set; } = "restricciones_rm";
    public string Cot { get; set; } = "cot";
    public string CfmjGfact { get; set; } = "cfm_j_fact";
}

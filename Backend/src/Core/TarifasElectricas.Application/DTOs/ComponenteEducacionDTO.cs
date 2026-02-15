namespace TarifasElectricas.Application.DTOs
{
    public class ComponenteEducacionDTO
    {
        public int Id { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string DescripcionSimple { get; set; } = string.Empty;
        public string Analogia { get; set; } = string.Empty;
        public string Icono { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public int? Orden { get; set; }
    }
}

using System;
using TarifasElectricas.Domain.Entities.EntityRoot;

namespace TarifasElectricas.Domain.Entities
{
    public class EducationComponent : Root
    {
        public string? Code { get; set; } = null!;
        public string? Name { get; set; } = null!;
        public string? SimpleDescription { get; set; } = null!;
        public string? Analogy { get; set; } = null!;
        public string? Icon { get; set; } = null!;
        public string? Color { get; set; } = null!;
        public int? Order { get; set; }
    }
}

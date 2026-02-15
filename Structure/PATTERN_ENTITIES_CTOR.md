# 🏗️ Patrón de Constructores para Entities

## Estructura Base

```csharp
using System;
using TarifasElectricas.Domain.Entities.EntityRoot;

namespace TarifasElectricas.Domain.Entities
{
    public class MyEntity : Root
    {
        // Propiedades
        public string? Property1 { get; set; }
        public int? Property2 { get; set; }
        public DateTime CreatedAt { get; set; }

        /// 
        /// Constructor privado para EF Core
        /// 
        private MyEntity() { }

        /// 
        /// Constructor público para crear una nueva entidad
        /// 
        public MyEntity(string? property1, int? property2)
        {
            Id = Guid.CreateVersion7();  // ← CLAVE: Generar Id único
            Property1 = property1;
            Property2 = property2;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
```
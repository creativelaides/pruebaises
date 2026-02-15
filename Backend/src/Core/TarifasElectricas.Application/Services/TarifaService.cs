using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TarifasElectricas.Application.DTOs;
using TarifasElectricas.Application.Interfaces;
using TarifasElectricas.Domain.Interfaces;

namespace TarifasElectricas.Application.Services
{
    public class TarifaService : ITarifaService
    {
        private readonly IElectricityTariffRepository _tarifaRepository;
        // private readonly IMapper _mapper; // Will be used later

        public TarifaService(IElectricityTariffRepository tarifaRepository)
        {
            _tarifaRepository = tarifaRepository;
        }

        public Task<IEnumerable<ComponenteEducacionDTO>> GetComponentesEducativosAsync()
        {
            // Dummy implementation
            var componentes = new List<ComponenteEducacionDTO>
            {
                new ComponenteEducacionDTO { Id = 1, Codigo = "C", Nombre = "Generación", DescripcionSimple = "Costo de producir la energía.", Analogia = "Ingredientes de la comida.", Icono = "⚡", Color = "#FFFFFF", Orden = 1 }
            };
            return Task.FromResult<IEnumerable<ComponenteEducacionDTO>>(componentes);
        }

        public Task<TarifaDTO> GetTarifaActualAsync()
        {
            // Dummy implementation
            var tarifa = new TarifaDTO
            {
                Año = DateTime.Now.Year,
                Mes = DateTime.Now.Month,
                CuTotal = 250.0m
            };
            return Task.FromResult(tarifa);
        }
    }
}

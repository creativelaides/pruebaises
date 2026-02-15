using System.Collections.Generic;
using System.Threading.Tasks;
using TarifasElectricas.Application.DTOs;

namespace TarifasElectricas.Application.Interfaces
{
    public interface ITarifaService
    {
        Task<TarifaDTO> GetTarifaActualAsync();
        Task<IEnumerable<ComponenteEducacionDTO>> GetComponentesEducativosAsync();
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;

namespace TarifasElectricas.Domain.Interfaces
{
    public interface ITariffRepository
    {
        Task BulkInsertAsync(IEnumerable<Entities.TarifaElectrica> tarifas);
        Task GuardarLogAsync(Entities.EtlLog log);
    }
}

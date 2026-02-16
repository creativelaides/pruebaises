using System.Runtime.CompilerServices;
using Mapster;
using TarifasElectricas.Application.Mapping;

namespace TarifasElectricas.Test;

internal static class MapsterTestConfig
{
    [ModuleInitializer]
    internal static void Init()
    {
        MapsterConfig.Register(TypeAdapterConfig.GlobalSettings);
    }
}

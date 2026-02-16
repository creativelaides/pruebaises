namespace TarifasElectricas.Infrastructure.Utils.Mapping;

/// <summary>
/// Comparador para TariffKey, útil para diccionarios.
/// </summary>
public sealed class TariffKeyComparer : IEqualityComparer<TariffKey>
{
    public static readonly TariffKeyComparer Instance = new();

    public bool Equals(TariffKey? x, TariffKey? y)
    {
        if (ReferenceEquals(x, y))
            return true;
        if (x is null || y is null)
            return false;
        return x.Year == y.Year &&
               x.Period == y.Period &&
               x.Level == y.Level &&
               x.TariffOperator == y.TariffOperator;
    }

    public int GetHashCode(TariffKey obj)
    {
        var hash = new HashCode();
        hash.Add(obj.Year);
        hash.Add(obj.Period);
        hash.Add(obj.Level);
        hash.Add(obj.TariffOperator);
        return hash.ToHashCode();
    }
}

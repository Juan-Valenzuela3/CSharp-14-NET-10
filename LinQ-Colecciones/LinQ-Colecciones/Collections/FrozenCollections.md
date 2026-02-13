# Frozen Collections
En .NET 10, cuando tienes datos que se cargan una vez y se consultan millones de veces (como una tabla de precios o configuración), usamos las Frozen Collections. Están ultra-optimizadas para búsquedas.

using System.Collections.Frozen;

namespace Performance.Collections;

public class ReferenceData(Dictionary<int, string> rawData)
{
    // Una colección "congelada" es mucho más rápida que un Dictionary normal
    // porque el compilador optimiza el acceso sabiendo que nunca cambiará.
    private readonly FrozenDictionary<int, string> _fastCache = rawData.ToFrozenDictionary();

    public string GetValue(int id) => _fastCache[id]; // Velocidad máxima O(1)
}
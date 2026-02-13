namespace LinQ_Colecciones.Examples.LinQ;

using Microsoft.EntityFrameworkCore;
using LinQ_Colecciones.Data;
using LinQ_Colecciones.Entities;

/// <summary>
/// Ejemplos de JOINS en LINQ
/// Combinar datos de múltiples colecciones
/// </summary>
public static class JoinsExample
{
    public static async Task Run()
    {
        var context = new AppDbContext(new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer("Server=localhost,1433;Database=LabRendimiento;User Id=sa;Password=SqlServer2024!Strong;TrustServerCertificate=True;MultipleActiveResultSets=true;")
            .Options);

        // Cargar datos
        var clientes = await context.Clientes.Take(10).ToListAsync();
        var pedidos = await context.Pedidos.Where(p => clientes.Select(c => c.Id).Contains(p.ClienteId)).ToListAsync();

        #region Inner Join
        var innerJoin = clientes
            .Join(
                pedidos,
                c => c.Id,
                p => p.ClienteId,
                (c, p) => new { c.Nombre, p.Monto }
            );
        #endregion

        #region Left Join
        var leftJoin = clientes
            .LeftJoin(
                pedidos,
                c => c.Id,
                p => p.ClienteId,
                (c, p) => new { c.Nombre, p.Monto }
            );
        #endregion

        #region Right Join
        var rightJoin = pedidos
            .RightJoin(
                clientes,
                p => p.ClienteId,
                c => c.Id,
                (p, c) => new { c.Nombre, p.Monto }
            );        
        #endregion

        #region CountBy
        var countByCliente = clientes
            .CountBy(c => pedidos.Count(p => p.ClienteId == c.Id));
        #endregion

        #region Secuencia LINQ completa
        IQueryable<Cliente> consultaCompleta = context.Clientes
            .Where(c => c.Nombre.StartsWith("A"))
            .Join(
                context.Pedidos,
                c => c.Id,
                p => p.ClienteId,
                (c, p) => new { c.Nombre, p.Monto }
            )
            .Select(cp => new Cliente { Nombre = cp.Nombre, Email = string.Empty })
            .Take(5)
            .Distinct(); // Elimina duplicados
        #endregion

        #region Count y Any
        // Count: Lee TODOS los elementos
        int totalPedidos = pedidos.Count;

        // Any: Lee solo hasta encontrar el primero
        bool hayPedidos = pedidos.Any();
        
        #endregion

        #region Chunk - Procesamiento por lotes
        var resumenLotes = pedidos.Chunk(5)
            .Select((lote, indice) => new 
            { 
                LoteNumero = indice + 1, 
                MontoTotal = lote.Sum(p => p.Monto),
                Cantidad = lote.Length
            });
        #endregion

        #region OrderBy - Operación costosa
        var pedidosOrdenados = pedidos
            .OrderBy(p => p.Monto)
            .ThenByDescending(p => p.FechaPedido)
            .Take(10);
        #endregion

        #region Materialización - ToList
        // ToList: Ideal para modificaciones posteriores
        var clientesEnMemoria = clientes
            .Where(c => c.Email.Contains("@"))
            .ToList();
        #endregion

        #region Materialización - ToArray
        // ToArray: Tamaño fijo, más rápido para lectura
        var pedidosArray = pedidos
            .Where(p => p.Monto > 500)
            .OrderByDescending(p => p.Monto)
            .ToArray();
        #endregion

        #region Materialización - ToDictionary
        // ToDictionary: Búsqueda O(1) por clave
        var cachePedidosPorCliente = pedidos
            .GroupBy(p => p.ClienteId)
            .ToDictionary(g => g.Key, g => g.ToList());
        #endregion
    }
}

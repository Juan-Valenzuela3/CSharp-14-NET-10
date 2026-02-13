namespace LinQ_Colecciones.Examples.LinQ;

using Microsoft.EntityFrameworkCore;
using LinQ_Colecciones.Data;
using System.Collections.Generic;
using LinQ_Colecciones.Entities;

/// <summary>
/// Ejemplos de Interfaces en LINQ
/// IEnumerable, ICollection, IQueryable, IAsyncEnumerable
/// </summary>
public static class InterfacesExample
{
    public static async Task Run()
    {
        var context = new AppDbContext(new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer("Server=localhost,1433;Database=LabRendimiento;User Id=sa;Password=SqlServer2024!Strong;TrustServerCertificate=True;MultipleActiveResultSets=true;")
            .Options);

        // Cargar datos
        var clientes = await context.Clientes.Take(10).ToListAsync();
        var pedidos = await context.Pedidos.Where(p => clientes.Select(c => c.Id).Contains(p.ClienteId)).ToListAsync();


        #region IEnumerable<T> - Iterador básico

        IEnumerable<decimal> montosEnumerable = context.Pedidos
            .Select(p => p.Monto)
            .Where(m => m > 1000); // Aún no se ejecuta

        // Aquí se ejecuta cuando iteramos
        var totalMontosAltos = montosEnumerable.Sum();
        #endregion

        #region ICollection<T> - Manipulador de datos
        // ICollection: Hereda de IEnumerable + Add, Remove, Count
        // Asume datos en memoria (RAM)
        // Permite modificar la estructura

        var clientesList = new List<string> { "Juan", "María", "Carlos" };
        ICollection<string> clientesCollection = clientesList;

        // Operaciones propias de ICollection
        clientesCollection.Add("Ana");
        clientesCollection.Remove("María");
        int cantidad = clientesCollection.Count;
        #endregion

        #region IQueryable<T> - Traductor a SQL
        // IQueryable: Traduce LINQ a SQL antes de ejecutar
        // Ejecución en el servidor (Base de datos)
        // Filtro ocurre en SQL, no en C#

        // Este Where se convierte en SQL y se ejecuta en la BD
        IQueryable<decimal> pedidosAltos = (IQueryable<decimal>)context.Pedidos
            .Where(p => p.Monto > 5000); // En SQL: WHERE Monto > 5000

        // Solo cuando se materializan los datos, se ejecuta
        // var resultado = pedidosAltos.ToList();
        #endregion

        #region IEnumerable vs IQueryable - Diferencia crítica
        // IEnumerable: Descarga TODO, luego filtra en RAM
        var pedidosDelServidor = await context.Pedidos.ToListAsync();
        IEnumerable<decimal> montosEnRAM = pedidosDelServidor
            .Where(p => p.Monto > 2000) // Se ejecuta en la APP (RAM)
            .Select(p => p.Monto);

        // IQueryable: Filtra en la BD, luego descarga
        IQueryable<decimal> montosEnSQL = context.Pedidos
            .Where(p => p.Monto > 2000) // Se ejecuta en SQL
            .Select(p => p.Monto);
        #endregion

        #region Combinación de IQueryable e IEnumerable
        // Primero IQueryable (en BD), luego IEnumerable (en RAM)
        var consultaMixta = context.Pedidos
            .Where(p => p.Monto > 1000) // IQueryable: En SQL
            .ToList() // Materializar: Trae datos a RAM
            .Where(p => p.FechaPedido.Year == 2025); // IEnumerable: En RAM
        #endregion

        #region IAsyncEnumerable<T> - Iteración asíncrona
        // IAsyncEnumerable: Permite await en foreach
        // Mientras se descarga un lote de la BD, el hilo está disponible para otras tareas
        // No bloquea el servidor esperando datos
        IAsyncEnumerable<int> idsClientes = context.Clientes       
        .Select(c => c.Id)
        .AsAsyncEnumerable();         
        #endregion
    }
}

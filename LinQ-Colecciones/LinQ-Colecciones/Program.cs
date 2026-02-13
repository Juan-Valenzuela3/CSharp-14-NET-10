using System.Diagnostics;
using Bogus;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using LinQ_Colecciones.Data;
using LinQ_Colecciones.Entities;

// ============================================================================
// LABORATORIO  - SQL Server + 1M Registros
// C# 14 / .NET 10
// ============================================================================

const string ConnectionString = 
    "Server=localhost,1433;" +
    "Database=LabRendimiento;" +
    "User Id=sa;" +
    "Password=SqlServer2024!Strong;" +
    "TrustServerCertificate=True;" +
    "MultipleActiveResultSets=true;";

const int TotalClientes = 100_000;
const int PedidosPorCliente = 10; // Total: 1,000,000 pedidos
const int BatchSize = 100_000;

Console.WriteLine("╔══════════════════════════════════════════════════════════════╗");
Console.WriteLine("║  🚀 LABORATORIO DE ALTO RENDIMIENTO - SQL Server             ║");
Console.WriteLine("║  📊 1,000,000 de registros con EF Core Bulk Extensions       ║");
Console.WriteLine("╚══════════════════════════════════════════════════════════════╝");
Console.WriteLine();

var stopwatchTotal = Stopwatch.StartNew();

// Configuración del DbContext
var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
optionsBuilder.UseSqlServer(ConnectionString);

await using var context = new AppDbContext(optionsBuilder.Options);

// 1. Crear base de datos si no existe
Console.WriteLine("📦 Verificando/Creando base de datos...");
var dbCreated = await context.Database.EnsureCreatedAsync();
Console.WriteLine(dbCreated ? "   ✅ Base de datos creada" : "   ✅ Base de datos ya existía");
Console.WriteLine();

// 2. Verificar si ya hay datos
var existingClientes = await context.Clientes.CountAsync();
if (existingClientes > 0)
{
    Console.WriteLine($"⚠️  La base de datos ya contiene {existingClientes:N0} clientes.");
    Console.WriteLine("   ¿Desea continuar y agregar más datos? (S/N)");
    var respuesta = Console.ReadLine()?.Trim().ToUpperInvariant();
    if (respuesta != "S")
    {
        Console.WriteLine("   Operación cancelada.");
        return;
    }
}

// 3. Configurar generadores Bogus
Console.WriteLine("🎲 Configurando generadores de datos falsos (Bogus)...");

var clienteIdCounter = existingClientes;
var pedidoIdCounter = await context.Pedidos.CountAsync();

var clienteFaker = new Faker<Cliente>("es")
    .RuleFor(c => c.Id, _ => ++clienteIdCounter)
    .RuleFor(c => c.Nombre, f => f.Person.FullName)
    .RuleFor(c => c.Email, f => f.Internet.Email())
    .RuleFor(c => c.FechaRegistro, f => f.Date.Past(3));

Console.WriteLine("   ✅ Generador de clientes configurado");
Console.WriteLine();

// 4. Inserción masiva en chunks
Console.WriteLine($"📤 Iniciando inserción masiva...");
Console.WriteLine($"   • Total clientes a generar: {TotalClientes:N0}");
Console.WriteLine($"   • Pedidos por cliente: {PedidosPorCliente}");
Console.WriteLine($"   • Total pedidos: {TotalClientes * PedidosPorCliente:N0}");
Console.WriteLine($"   • Tamaño de batch: {BatchSize:N0}");
Console.WriteLine();

var stopwatchInsercion = Stopwatch.StartNew();
var totalClientesInsertados = 0;
var totalPedidosInsertados = 0;

// Procesar clientes en batches
var clientesBatchCount = (int)Math.Ceiling((double)TotalClientes / BatchSize);

for (var batch = 0; batch < clientesBatchCount; batch++)
{
    var clientesEnEsteBatch = Math.Min(BatchSize, TotalClientes - (batch * BatchSize));
    
    Console.WriteLine($"   📦 Batch {batch + 1}/{clientesBatchCount}: Generando {clientesEnEsteBatch:N0} clientes...");
    
    // Generar clientes del batch actual
    List<Cliente> clientes = clienteFaker.Generate(clientesEnEsteBatch);
    
    // Insertar clientes
    var stopwatchBatch = Stopwatch.StartNew();
    await context.BulkInsertAsync(clientes, new BulkConfig 
    { 
        SetOutputIdentity = true,
        BatchSize = 50_000
    });
    
    totalClientesInsertados += clientes.Count;
    Console.WriteLine($"      ✅ Clientes insertados en {stopwatchBatch.ElapsedMilliseconds}ms");
    
    // Generar pedidos para cada cliente de este batch
    Console.WriteLine($"      📝 Generando pedidos para {clientes.Count:N0} clientes...");
    
    List<Pedido> pedidos = [];
    var random = new Random();
    
    foreach (var cliente in clientes)
    {
        for (var i = 0; i < PedidosPorCliente; i++)
        {
            pedidos.Add(new Pedido
            {
                Id = ++pedidoIdCounter,
                ClienteId = cliente.Id,
                Monto = Math.Round((decimal)(random.NextDouble() * 10000 + 10), 2),
                FechaPedido = cliente.FechaRegistro.AddDays(random.Next(1, 365))
            });
        }
    }
    
    // Insertar pedidos en sub-batches si es necesario
    var pedidosBatchSize = 100_000;
    for (var i = 0; i < pedidos.Count; i += pedidosBatchSize)
    {
        var pedidosSubBatch = pedidos.Skip(i).Take(pedidosBatchSize).ToList();
        await context.BulkInsertAsync(pedidosSubBatch, new BulkConfig { BatchSize = 50_000 });
    }
    
    totalPedidosInsertados += pedidos.Count;
    stopwatchBatch.Stop();
    
    Console.WriteLine($"      ✅ {pedidos.Count:N0} pedidos insertados en {stopwatchBatch.ElapsedMilliseconds}ms");
    
    // Limpiar memoria
    clientes.Clear();
    pedidos.Clear();
    GC.Collect();
    
    Console.WriteLine($"      📊 Progreso: {totalClientesInsertados:N0}/{TotalClientes:N0} clientes, {totalPedidosInsertados:N0} pedidos");
    Console.WriteLine();
}

stopwatchInsercion.Stop();
stopwatchTotal.Stop();

// 5. Resumen final
Console.WriteLine("╔══════════════════════════════════════════════════════════════╗");
Console.WriteLine("║  📊 RESUMEN DE LA OPERACIÓN                                  ║");
Console.WriteLine("╠══════════════════════════════════════════════════════════════╣");
Console.WriteLine($"║  Clientes insertados:     {totalClientesInsertados,15:N0}               ║");
Console.WriteLine($"║  Pedidos insertados:      {totalPedidosInsertados,15:N0}               ║");
Console.WriteLine($"║  Total registros:         {totalClientesInsertados + totalPedidosInsertados,15:N0}               ║");
Console.WriteLine("╠══════════════════════════════════════════════════════════════╣");
Console.WriteLine($"║  ⏱️  Tiempo de inserción:  {stopwatchInsercion.Elapsed:hh\\:mm\\:ss\\.fff}                       ║");
Console.WriteLine($"║  ⏱️  Tiempo total:         {stopwatchTotal.Elapsed:hh\\:mm\\:ss\\.fff}                       ║");
Console.WriteLine("╚══════════════════════════════════════════════════════════════╝");

// 6. Verificación rápida
Console.WriteLine();
Console.WriteLine("🔍 Verificación de datos:");
var totalClientesDb = await context.Clientes.CountAsync();
var totalPedidosDb = await context.Pedidos.CountAsync();
var promedioMonto = await context.Pedidos.AverageAsync(p => p.Monto);

Console.WriteLine($"   • Clientes en BD: {totalClientesDb:N0}");
Console.WriteLine($"   • Pedidos en BD: {totalPedidosDb:N0}");
Console.WriteLine($"   • Monto promedio: ${promedioMonto:N2}");
Console.WriteLine();
Console.WriteLine("✅ Laboratorio listo para pruebas de rendimiento!");

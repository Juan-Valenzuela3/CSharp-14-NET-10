using Microsoft.EntityFrameworkCore;
using LinQ_Colecciones.Entities;

namespace LinQ_Colecciones.Data;

/// <summary>
/// DbContext configurado para SQL Server con relación 1:N entre Cliente y Pedido
/// </summary>
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<Pedido> Pedidos => Set<Pedido>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configuración de Cliente
        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Nombre).HasMaxLength(100).IsRequired();
            entity.Property(c => c.Email).HasMaxLength(150).IsRequired();
            entity.Property(c => c.FechaRegistro).IsRequired();
            
            // Índice en Email para búsquedas rápidas
            entity.HasIndex(c => c.Email);
        });

        // Configuración de Pedido
        modelBuilder.Entity<Pedido>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Monto).HasPrecision(18, 2).IsRequired();
            entity.Property(p => p.FechaPedido).IsRequired();
            
            // Relación 1:N con Cliente
            entity.HasOne(p => p.Cliente)
                  .WithMany(c => c.Pedidos)
                  .HasForeignKey(p => p.ClienteId)
                  .OnDelete(DeleteBehavior.Cascade);
            
            // Índice en ClienteId para JOINs rápidos
            entity.HasIndex(p => p.ClienteId);
            entity.HasIndex(p => p.FechaPedido);
        });
    }
}

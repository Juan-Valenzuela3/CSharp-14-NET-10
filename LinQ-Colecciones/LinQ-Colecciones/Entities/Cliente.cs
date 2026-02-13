namespace LinQ_Colecciones.Entities;

/// <summary>
/// Entidad Cliente con Primary Constructor (C# 14)
/// Relación 1:N con Pedido
/// </summary>
public class Cliente
{
    public int Id { get; set; }
    public required string Nombre { get; set; }
    public required string Email { get; set; }
    public DateTime FechaRegistro { get; set; }
    
    // Navegación: Un cliente tiene muchos pedidos
    public List<Pedido> Pedidos { get; set; } = [];
}

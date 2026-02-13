namespace LinQ_Colecciones.Entities;

/// <summary>
/// Entidad Pedido con Primary Constructor (C# 14)
/// Relación N:1 con Cliente
/// </summary>
public class Pedido
{
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public decimal Monto { get; set; }
    public DateTime FechaPedido { get; set; }
    
    // Navegación: Un pedido pertenece a un cliente
    public Cliente? Cliente { get; set; }
}

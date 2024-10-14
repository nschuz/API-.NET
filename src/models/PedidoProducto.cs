namespace TiendaOnlineAPI.Models
{
public class PedidoProducto
{
    public int PedidoId { get; set; }
    public Pedido? Pedido { get; set; } // Nullable si es opcional

    public int ProductoId { get; set; }
    public Producto? Producto { get; set; } // Nullable si es opcional

    public int Cantidad { get; set; }
}
}

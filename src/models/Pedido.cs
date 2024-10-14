using System;
using System.Collections.Generic;

namespace TiendaOnlineAPI.Models
{
public class Pedido
{
    public int Id { get; set; }
    public DateTime Fecha { get; set; }
    public int ClienteId { get; set; }
    public ICollection<PedidoProducto> PedidoProductos { get; set; } = new List<PedidoProducto>(); // Inicialización
}

}

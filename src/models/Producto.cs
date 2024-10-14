using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TiendaOnlineAPI.Models
{

public class Producto
{
    public int Id { get; set; }

    [Required]
    public string Nombre { get; set; } = string.Empty;  /

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor que cero.")]
    public decimal Precio { get; set; } 

    public ICollection<PedidoProducto> PedidoProductos { get; set; } = new List<PedidoProducto>(); /
}

}

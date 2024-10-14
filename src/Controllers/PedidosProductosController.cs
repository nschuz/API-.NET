using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TiendaOnlineAPI.Data;
using TiendaOnlineAPI.Models;

[ApiController]
[Route("pedidos/{pedidoId}/productos")]
public class PedidoProductosController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public PedidoProductosController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: /pedidos/{pedidoId}/productos
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PedidoProducto>>> GetProductosPorPedido(int pedidoId)
    {
        var pedidoProductos = await _context.PedidoProductos
            .Include(pp => pp.Producto)
            .Where(pp => pp.PedidoId == pedidoId)
            .ToListAsync();

        return pedidoProductos;
    }

    // POST: /pedidos/{pedidoId}/productos
    [HttpPost]
    public async Task<ActionResult<PedidoProducto>> PostProductoEnPedido(int pedidoId, PedidoProducto pedidoProducto)
    {
        if (pedidoId != pedidoProducto.PedidoId)
        {
            return BadRequest();
        }

        _context.PedidoProductos.Add(pedidoProducto);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetProductosPorPedido), new { pedidoId = pedidoId }, pedidoProducto);
    }
}

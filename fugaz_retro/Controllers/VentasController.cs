
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using fugaz_retro.Models;
using Microsoft.AspNetCore.Authorization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace fugaz_retro.Controllers
{
    [Authorize]
    public class VentasController : Controller
    {
        private readonly FugazContext _context;

        public VentasController(FugazContext context)
        {
            _context = context;
        }

        // GET: Ventas
        public async Task<IActionResult> Index()
        {
            var ventasPorCliente = await _context.Ventas
                .Include(v => v.IdPedidoNavigation)
                    .ThenInclude(p => p.IdClienteNavigation) // Incluye la navegación al cliente desde el pedido
                .GroupBy(v => v.IdPedidoNavigation.IdClienteNavigation.Usuarios.NombreUsuario) // Agrupar por nombre del cliente
                .ToListAsync();

            return View(ventasPorCliente);
        }


        // GET: Ventas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var venta = await _context.Ventas
                  .Include(v => v.IdPedidoNavigation)
                      .ThenInclude(p => p.IdClienteNavigation) // Incluir cliente
                          .ThenInclude(c => c.Usuarios) // Incluir usuario
                  .FirstOrDefaultAsync(m => m.IdVenta == id);

            if (venta == null)
            {
                return NotFound();
            }

            string nombreCliente = "Cliente no encontrado";

            if (venta.IdPedidoNavigation != null && venta.IdPedidoNavigation.IdClienteNavigation != null && venta.IdPedidoNavigation.IdClienteNavigation.Usuarios != null)
            {
                nombreCliente = venta.IdPedidoNavigation.IdClienteNavigation.Usuarios.NombreUsuario;
            }

            // Obtener todos los pedidos entregados asociados a este cliente
            var pedidosEntregados = await _context.Pedidos
                .Where(p => p.IdCliente == venta.IdPedidoNavigation.IdCliente)
                .Where(p => p.Estado == "Entregado") // Filtrar por estado entregado
                .ToListAsync();

            ViewBag.NombreCliente = nombreCliente;
            ViewBag.PedidosEntregados = pedidosEntregados;

            return View(venta);
        }



        // GET: Ventas/Create
        public IActionResult Create()
        {
            ViewData["IdPedido"] = new SelectList(_context.Pedidos, "IdPedido", "FechaPedido");

            return View();
        }

        // POST: Ventas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdVenta,IdPedido,Subtotal,Iva,Decuento,TotalVenta")] Venta venta)
        {
            if (ModelState.IsValid)
            {
                _context.Add(venta);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdPedido"] = new SelectList(_context.Pedidos, "IdPedido", "FechaPedido", venta.IdPedido);
            return View(venta);
        }

        // GET: Ventas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Ventas == null)
            {
                return NotFound();
            }

            var venta = await _context.Ventas.FindAsync(id);
            if (venta == null)
            {
                return NotFound();
            }
            ViewData["IdPedido"] = new SelectList(_context.Pedidos, "IdPedido", "IdPedido", venta.IdPedido);

            return View(venta);
        }

        // POST: Ventas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdVenta,IdUsuarrio,IdPedido,Subtotal,Iva,Decuento,TotalVenta")] Venta venta)
        {
            if (id != venta.IdVenta)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(venta);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VentaExists(venta.IdVenta))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdPedido"] = new SelectList(_context.Pedidos, "IdPedido", "IdPedido", venta.IdPedido);

            return View(venta);
        }

        // GET: Ventas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Ventas == null)
            {
                return NotFound();
            }

            var venta = await _context.Ventas
                .Include(v => v.IdPedidoNavigation)

                .FirstOrDefaultAsync(m => m.IdVenta == id);
            if (venta == null)
            {
                return NotFound();
            }

            return View(venta);
        }

        // POST: Ventas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Ventas == null)
            {
                return Problem("Entity set 'FugazContext.Ventas'  is null.");
            }
            var venta = await _context.Ventas.FindAsync(id);
            if (venta != null)
            {
                _context.Ventas.Remove(venta);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VentaExists(int id)
        {
            return (_context.Ventas?.Any(e => e.IdVenta == id)).GetValueOrDefault();
        }
    }
}

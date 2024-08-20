using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using fugaz_retro.Models;
using Microsoft.AspNetCore.Authorization;

namespace fugaz_retro.Controllers
{
    [Authorize]
    public class DetallePedidosController : Controller
    {
        private readonly FugazContext _context;

        public DetallePedidosController(FugazContext context)
        {
            _context = context;
        }

        // GET: DetallePedidos
        public async Task<IActionResult> Index()
        {
            var detallePedidos = await _context.DetallePedidos
                                            .Include(dp => dp.IdPedidoNavigation) 
                                            .ThenInclude(p => p.IdClienteNavigation) 
                                            .ThenInclude(c => c.Usuarios) 
                                            .ToListAsync();

            return View(detallePedidos);
        }

        // GET: DetallePedidos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.DetallePedidos == null)
            {
                return NotFound();
            }

            var detallePedido = await _context.DetallePedidos
                .Include(d => d.IdPedidoNavigation)
                //.Include(d => d.IdProductoNavigation)
                .FirstOrDefaultAsync(m => m.IdDetallePedido == id);
            if (detallePedido == null)
            {
                return NotFound();
            }

            return View(detallePedido);
        }

        public IActionResult Create()
        {
            ViewBag.IdPedido = new SelectList(_context.Pedidos, "IdPedido", "FechaPedido"); 
            ViewBag.IdDetalleProducto = new SelectList(_context.DetalleProductos.Include(dp => dp.Producto), "IdDetalleProducto", "Producto.NombreProducto");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DetallePedido detallePedido)
        {
            if (ModelState.IsValid)
            {
                _context.Add(detallePedido);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.IdPedido = new SelectList(_context.Pedidos, "IdPedido", "FechaPedido", detallePedido.IdPedido);
            ViewBag.IdDetalleProducto = new SelectList(_context.DetalleProductos.Include(dp => dp.Producto), "IdDetalleProducto", "Producto.NombreProducto", detallePedido.IdDetalleProducto);
            return View(detallePedido);
        }


        // GET: DetallePedidos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.DetallePedidos == null)
            {
                return NotFound();
            }

            var detallePedido = await _context.DetallePedidos.FindAsync(id);
            if (detallePedido == null)
            {
                return NotFound();
            }
            ViewData["IdPedido"] = new SelectList(_context.Pedidos, "IdPedido", "IdPedido", detallePedido.IdPedido);
            //ViewData["IdProducto"] = new SelectList(_context.Productos, "IdProducto", "IdProducto", detallePedido.IdProducto);
            return View(detallePedido);
        }

        // POST: DetallePedidos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdDetallePedido,IdPedido,IdProducto,Cantidad,PrecioUnitario")] DetallePedido detallePedido)
        {
            if (id != detallePedido.IdDetallePedido)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(detallePedido);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DetallePedidoExists(detallePedido.IdDetallePedido))
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
            ViewData["IdPedido"] = new SelectList(_context.Pedidos, "IdPedido", "IdPedido", detallePedido.IdPedido);
            //ViewData["IdProducto"] = new SelectList(_context.Productos, "IdProducto", "IdProducto", detallePedido.IdProducto);
            return View(detallePedido);
        }

        // GET: DetallePedidos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.DetallePedidos == null)
            {
                return NotFound();
            }

            var detallePedido = await _context.DetallePedidos
                .Include(d => d.IdPedidoNavigation)
                //.Include(d => d.IdProductoNavigation)
                .FirstOrDefaultAsync(m => m.IdDetallePedido == id);
            if (detallePedido == null)
            {
                return NotFound();
            }

            return View(detallePedido);
        }

        // POST: DetallePedidos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.DetallePedidos == null)
            {
                return Problem("Entity set 'FugazContext.DetallePedidos'  is null.");
            }
            var detallePedido = await _context.DetallePedidos.FindAsync(id);
            if (detallePedido != null)
            {
                _context.DetallePedidos.Remove(detallePedido);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DetallePedidoExists(int id)
        {
          return (_context.DetallePedidos?.Any(e => e.IdDetallePedido == id)).GetValueOrDefault();
        }
    }
}

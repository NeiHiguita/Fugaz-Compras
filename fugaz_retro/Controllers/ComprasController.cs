using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using fugaz_retro.Models;

namespace fugaz_retro.Controllers
{
    public class ComprasController : Controller
    {
        private readonly FugazContext _context;

        public ComprasController(FugazContext context)
        {
            _context = context;
        }

        // GET: Compras
        public IActionResult Index()
        {
            var compras = _context.Compras
                .Include(c => c.IdProveedorNavigation)
                .ToList();

            return View(compras);
        }

        // GET: Compras/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var compra = await _context.Compras
                .Include(c => c.IdProveedorNavigation)
                .Include(c => c.DetalleCompras)
                    .ThenInclude(dc => dc.IdInsumoNavigation)
                .FirstOrDefaultAsync(m => m.IdCompra == id);

            if (compra == null)
            {
                return NotFound();
            }

            return View(compra);
        }

        // GET: Compras/Create
        public IActionResult Create()
        {
            ViewBag.Proveedores = _context.Proveedors.ToList();
            ViewBag.Insumos = _context.Insumos.ToList();

            var model = new Compra
            {
                DetalleCompras = new List<DetalleCompra>()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Compra compra, List<DetalleCompra> detallesCompra)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var cultura = CultureInfo.InvariantCulture;

                    foreach (var detalle in detallesCompra)
                    {
                        var cantidadStr = Request.Form[$"DetalleCompras[{detallesCompra.IndexOf(detalle)}].Cantidad"];
                        if (decimal.TryParse(cantidadStr, NumberStyles.AllowDecimalPoint, cultura, out var cantidad))
                        {
                            detalle.Cantidad = cantidad;
                        }
                        else
                        {
                            ModelState.AddModelError($"DetalleCompras[{detallesCompra.IndexOf(detalle)}].Cantidad", $"Cantidad inválida: {cantidadStr}");
                        }
                    }

                    if (ModelState.IsValid)
                    {
                        _context.Compras.Add(compra);
                        await _context.SaveChangesAsync();

                        foreach (var detalle in detallesCompra)
                        {
                            detalle.IdCompra = compra.IdCompra;
                            _context.DetalleCompras.Add(detalle);
                        }

                        await _context.SaveChangesAsync();

                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception ex)
                {
                    // Agregar más detalles al mensaje de error
                    ModelState.AddModelError(string.Empty, "Ocurrió un error al crear la compra: " + ex.Message);
                }
            }

            // Enviar datos necesarios a la vista si el modelo no es válido
            ViewBag.Proveedores = _context.Proveedors.ToList();
            ViewBag.Insumos = _context.Insumos.ToList();
            return View(compra);
        }

        // GET: Compras/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null || _context.Compras == null)
        //    {
        //        return NotFound();
        //    }

        //    var compra = await _context.Compras.FindAsync(id);
        //    if (compra == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["IdProveedor"] = new SelectList(_context.Proveedors, "IdProveedor", "TipoProveedor", compra.IdProveedor);
        //    return View(compra);
        //}

        // POST: Compras/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("IdCompra,IdUsuario,IdProveedor,FechaCompra,MetodoPago,Subtotal,Iva,Descuento,PrecioTotal")] Compra compra)
        //{
        //    if (id != compra.IdCompra)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(compra);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!CompraExists(compra.IdCompra))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["IdProveedor"] = new SelectList(_context.Proveedors, "IdProveedor", "IdProveedor", compra.IdProveedor);
        //    return View(compra);
        //}

        // GET: Compras/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Compras == null)
            {
                return NotFound();
            }

            var compra = await _context.Compras
                .Include(c => c.IdProveedorNavigation)
                .FirstOrDefaultAsync(m => m.IdCompra == id);
            if (compra == null)
            {
                return NotFound();
            }

            return View(compra);
        }

        // POST: Compras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Compras == null)
            {
                return Problem("Entity set 'FugazContext.Compras' is null.");
            }
            var compra = await _context.Compras.FindAsync(id);
            if (compra != null)
            {
                _context.Compras.Remove(compra);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompraExists(int id)
        {
            return (_context.Compras?.Any(e => e.IdCompra == id)).GetValueOrDefault();
        }
    }
}

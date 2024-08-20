using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using fugaz_retro.Models;

namespace fugaz_retro.Controllers
{
    [Authorize]
    public class ProductosController : Controller
    {
        private readonly FugazContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ProductosController(FugazContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: Productos
        public async Task<IActionResult> Index()
        {
            return _context.Productos != null ?
                        View(await _context.Productos.ToListAsync()) :
                        Problem("Entity set 'FugazContext.Productos'  is null.");
        }

        // GET: Producto/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos
                .Include(p => p.DetalleProductos)
                .FirstOrDefaultAsync(m => m.IdProducto == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // GET: Productos/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Producto producto, string tallasHiddenInput, string coloresHiddenInput, IFormFile Foto)
        {
            if (ModelState.IsValid)
            {
                if (Foto != null && Foto.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await Foto.CopyToAsync(memoryStream);
                        producto.Foto = memoryStream.ToArray();
                    }
                }

                _context.Add(producto);
                await _context.SaveChangesAsync();

                if (!string.IsNullOrEmpty(tallasHiddenInput))
                {
                    var tallas = JsonConvert.DeserializeObject<List<DetalleProducto>>(tallasHiddenInput);
                    if (tallas != null)
                    {
                        foreach (var detalle in tallas)
                        {
                            detalle.IdProducto = producto.IdProducto;
                            _context.DetalleProductos.Add(detalle);
                        }
                    }
                }

                if (!string.IsNullOrEmpty(coloresHiddenInput))
                {
                    var colores = JsonConvert.DeserializeObject<List<DetalleProducto>>(coloresHiddenInput);
                    if (colores != null)
                    {
                        foreach (var detalle in colores)
                        {
                            detalle.IdProducto = producto.IdProducto;
                            _context.DetalleProductos.Add(detalle);
                        }
                    }
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(producto);
        }

        // GET: Producto/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos
                .Include(p => p.DetalleProductos)
                .FirstOrDefaultAsync(p => p.IdProducto == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Producto producto, List<DetalleProducto> DetalleProductos, IFormFile Foto)
        {
            if (id != producto.IdProducto)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        if (Foto != null && Foto.Length > 0)
                        {
                            using (var memoryStream = new MemoryStream())
                            {
                                await Foto.CopyToAsync(memoryStream);
                                producto.Foto = memoryStream.ToArray();
                            }
                        }
                        else
                        {
                            var existingProduct = await _context.Productos.AsNoTracking().FirstOrDefaultAsync(p => p.IdProducto == id);
                            if (existingProduct != null)
                            {
                                producto.Foto = existingProduct.Foto;
                            }
                        }

                        _context.Update(producto);
                        await _context.SaveChangesAsync();

                        var existingDetalleProductos = _context.DetalleProductos.Where(dp => dp.IdProducto == id).ToList();
                        _context.DetalleProductos.RemoveRange(existingDetalleProductos);
                        await _context.SaveChangesAsync();

                        foreach (var detalle in DetalleProductos)
                        {
                            detalle.IdProducto = producto.IdProducto;
                            _context.DetalleProductos.Add(detalle);
                        }

                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();

                        return RedirectToAction(nameof(Index));
                    }
                    catch (Exception)
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                }
            }

            return View(producto);
        }


        // GET: Producto/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos
                .Include(p => p.DetalleProductos)
                .FirstOrDefaultAsync(m => m.IdProducto == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // POST: Producto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var producto = await _context.Productos
                .Include(p => p.DetalleProductos)
                .FirstOrDefaultAsync(p => p.IdProducto == id);

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    if (producto != null)
                    {
                        _context.DetalleProductos.RemoveRange(producto.DetalleProductos);
                        _context.Productos.Remove(producto);

                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();
                    }

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        private bool ProductoExists(int id)
        {
            return (_context.Productos?.Any(e => e.IdProducto == id)).GetValueOrDefault();
        }
    }
}
using System;
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
    public class PerdidaInsumosController : Controller
    {
        private readonly FugazContext _context;

        public PerdidaInsumosController(FugazContext context)
        {
            _context = context;
        }

        // GET: PerdidaInsumos
        public async Task<IActionResult> Index()
        {
            var fugazContext = _context.PerdidaInsumos.Include(p => p.IdInsumoNavigation);
            return View(await fugazContext.ToListAsync());
        }

        // GET: PerdidaInsumos/Details/5
        // GET: PerdidaInsumos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PerdidaInsumos == null)
            {
                return NotFound();
            }

            var perdidaInsumo = await _context.PerdidaInsumos
                .Include(p => p.IdInsumoNavigation)
                .FirstOrDefaultAsync(m => m.IdPerdidaInsumo == id);
            if (perdidaInsumo == null)
            {
                return NotFound();
            }

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new
                {
                    idPerdidaInsumo = perdidaInsumo.IdPerdidaInsumo,
                    cantidad = perdidaInsumo.Cantidad,
                    unidadMedida = perdidaInsumo.UnidadMedida,
                    fecha = perdidaInsumo.Fecha,
                    idInsumoNavigation = new
                    {
                        nombreInsumo = perdidaInsumo.IdInsumoNavigation.NombreInsumo
                    }
                });
            }

            return View(perdidaInsumo);
        }

        [HttpGet]
        public JsonResult FiltrarInsumosPorCategoria(string categoria)
        {
            var insumos = _context.Insumos
                .Where(i => i.CategoriaInsumo == categoria)
                .Select(i => new
                {
                    i.IdInsumo,
                    i.NombreInsumo
                }).ToList();

            return Json(insumos);
        }

        // GET: PerdidaInsumos/Create
        public IActionResult Create()
        {
            ViewData["IdInsumo"] = new SelectList(_context.Insumos, "IdInsumo", "NombreInsumo");
            return View();
        }

        // POST: PerdidaInsumos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPerdidaInsumo,IdInsumo,Cantidad,UnidadMedida,Fecha")] PerdidaInsumo perdidaInsumo)
        {
            if (ModelState.IsValid)
            {
                // Buscar el insumo asociado
                var insumo = await _context.Insumos.FindAsync(perdidaInsumo.IdInsumo);
                if (insumo == null)
                {
                    ModelState.AddModelError(string.Empty, "Insumo no encontrado.");
                    ViewData["IdInsumo"] = new SelectList(_context.Insumos, "IdInsumo", "NombreInsumo", perdidaInsumo.IdInsumo);
                    return View(perdidaInsumo);
                }

                // Validar la cantidad ingresada
                //if (perdidaInsumo.Cantidad > insumo.Stock)
                //{
                //    ModelState.AddModelError("Cantidad", "La cantidad no puede ser mayor que el stock disponible.");
                //    ViewData["IdInsumo"] = new SelectList(_context.Insumos, "IdInsumo", "NombreInsumo", perdidaInsumo.IdInsumo);
                //    return View(perdidaInsumo);
                //}

                // Restar la cantidad perdida del stock
                //insumo.Stock -= perdidaInsumo.Cantidad;

                // Actualizar el estado del insumo según el nuevo stock
                insumo.Estado = insumo.Stock > 3 ? "Disponible" : "Agotado";

                try
                {
                    // Actualizar el insumo y agregar la pérdida
                    _context.Update(insumo);
                    _context.Add(perdidaInsumo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Manejo de excepciones en caso de problemas con la actualización de datos
                    ModelState.AddModelError(string.Empty, "Hubo un problema al guardar los cambios. Intenta nuevamente.");
                    ViewData["IdInsumo"] = new SelectList(_context.Insumos, "IdInsumo", "NombreInsumo", perdidaInsumo.IdInsumo);
                    return View(perdidaInsumo);
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["IdInsumo"] = new SelectList(_context.Insumos, "IdInsumo", "NombreInsumo", perdidaInsumo.IdInsumo);
            return View(perdidaInsumo);
        }

        [HttpPost]
        public async Task<IActionResult> Anular(int id, int cantidad, int idInsumo)
        {
            try
            {
                // Buscar la pérdida de insumo por ID
                var perdidaInsumo = await _context.PerdidaInsumos.FindAsync(id);
                if (perdidaInsumo == null)
                {
                    return NotFound();
                }

                // Actualizar el stock del insumo
                var insumo = await _context.Insumos.FindAsync(idInsumo);
                if (insumo == null)
                {
                    return NotFound();
                }

                insumo.Stock += cantidad; // Devolver la cantidad al stock

                // Eliminar la pérdida de insumo
                _context.PerdidaInsumos.Remove(perdidaInsumo);
                await _context.SaveChangesAsync();

                return Json(new { success = true });
            }
            catch
            {
                return Json(new { success = false });
            }
        }

        // GET: PerdidaInsumos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PerdidaInsumos == null)
            {
                return NotFound();
            }

            var perdidaInsumo = await _context.PerdidaInsumos.FindAsync(id);
            if (perdidaInsumo == null)
            {
                return NotFound();
            }
            ViewData["IdInsumo"] = new SelectList(_context.Insumos, "IdInsumo", "NombreInsumo", perdidaInsumo.IdInsumo);
            return View(perdidaInsumo);
        }

        // POST: PerdidaInsumos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdPerdidaInsumo,IdInsumo,Cantidad,Fecha")] PerdidaInsumo perdidaInsumo)
        {
            if (id != perdidaInsumo.IdPerdidaInsumo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(perdidaInsumo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PerdidaInsumoExists(perdidaInsumo.IdPerdidaInsumo))
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
            ViewData["IdInsumo"] = new SelectList(_context.Insumos, "IdInsumo", "NombreInsumo", perdidaInsumo.IdInsumo);
            return View(perdidaInsumo);
        }

        // GET: PerdidaInsumos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.PerdidaInsumos == null)
            {
                return NotFound();
            }

            var perdidaInsumo = await _context.PerdidaInsumos
                .Include(p => p.IdInsumoNavigation)
                .FirstOrDefaultAsync(m => m.IdPerdidaInsumo == id);
            if (perdidaInsumo == null)
            {
                return NotFound();
            }

            return View(perdidaInsumo);
        }

        // POST: PerdidaInsumos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PerdidaInsumos == null)
            {
                return Problem("Entity set 'FugazContext.PerdidaInsumos' is null.");
            }
            var perdidaInsumo = await _context.PerdidaInsumos.FindAsync(id);
            if (perdidaInsumo != null)
            {
                _context.PerdidaInsumos.Remove(perdidaInsumo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PerdidaInsumoExists(int id)
        {
            return (_context.PerdidaInsumos?.Any(e => e.IdPerdidaInsumo == id)).GetValueOrDefault();
        }

        // GET: PerdidaInsumos/GetStock
        [HttpGet]
        public JsonResult GetStock(int id)
        {
            var insumo = _context.Insumos.Find(id);
            if (insumo != null)
            {
                return Json(new { stock = insumo.Stock });
            }
            return Json(new { stock = 0 });
        }

    }
}

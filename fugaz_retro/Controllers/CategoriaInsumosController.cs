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
    public class CategoriaInsumosController : Controller
    {
        private readonly FugazContext _context;

        public CategoriaInsumosController(FugazContext context)
        {
            _context = context;
        }

        // GET: CategoriaInsumos
        public async Task<IActionResult> Index()
        {
            return _context.CategoriaInsumos != null ?
                        View(await _context.CategoriaInsumos.ToListAsync()) :
                        Problem("Entity set 'FugazContext.CategoriaInsumos'  is null.");
        }

        // GET: CategoriaInsumos/Details/5
        // GET: CategoriaInsumos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CategoriaInsumos == null)
            {
                return NotFound();
            }

            var categoriaInsumo = await _context.CategoriaInsumos
                .FirstOrDefaultAsync(m => m.IdCategoria == id);
            if (categoriaInsumo == null)
            {
                return NotFound();
            }

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                // Devuelve los detalles en formato JSON si la solicitud es AJAX
                return Json(new
                {
                    nombreCategoria = categoriaInsumo.NombreCategoria,
                    estadoCategoria = categoriaInsumo.EstadoCategoria
                });
            }

            return View(categoriaInsumo);
        }


        // GET: CategoriaInsumos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CategoriaInsumos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCategoria,NombreCategoria,EstadoCategoria")] CategoriaInsumo categoriaInsumo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(categoriaInsumo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(categoriaInsumo);
        }
        // POST: CategoriaInsumos/ToggleEstado/5
        [HttpPost]
        public async Task<IActionResult> ToggleEstado(int id)
        {
            var categoriaInsumo = await _context.CategoriaInsumos.FindAsync(id);
            if (categoriaInsumo == null)
            {
                return NotFound();
            }

            categoriaInsumo.EstadoCategoria = !categoriaInsumo.EstadoCategoria;
            _context.Update(categoriaInsumo);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // GET: CategoriaInsumos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CategoriaInsumos == null)
            {
                return NotFound();
            }

            var categoriaInsumo = await _context.CategoriaInsumos.FindAsync(id);
            if (categoriaInsumo == null)
            {
                return NotFound();
            }
            return View(categoriaInsumo);
        }

        // POST: CategoriaInsumos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCategoria,NombreCategoria,EstadoCategoria")] CategoriaInsumo categoriaInsumo)
        {
            if (id != categoriaInsumo.IdCategoria)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(categoriaInsumo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoriaInsumoExists(categoriaInsumo.IdCategoria))
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
            return View(categoriaInsumo);
        }

        // GET: CategoriaInsumos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CategoriaInsumos == null)
            {
                return NotFound();
            }

            var categoriaInsumo = await _context.CategoriaInsumos
                .FirstOrDefaultAsync(m => m.IdCategoria == id);
            if (categoriaInsumo == null)
            {
                return NotFound();
            }

            return View(categoriaInsumo);
        }

        // POST: CategoriaInsumos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CategoriaInsumos == null)
            {
                return Problem("Entity set 'FugazContext.CategoriaInsumos'  is null.");
            }
            var categoriaInsumo = await _context.CategoriaInsumos.FindAsync(id);
            if (categoriaInsumo != null)
            {
                _context.CategoriaInsumos.Remove(categoriaInsumo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: CategoriaInsumos/GetCategoriaInsumoDetails/5
        public async Task<IActionResult> GetCategoriaInsumoDetails(int? id)
        {
            if (id == null || _context.CategoriaInsumos == null)
            {
                return NotFound();
            }

            var categoriaInsumo = await _context.CategoriaInsumos.FirstOrDefaultAsync(m => m.IdCategoria == id);
            if (categoriaInsumo == null)
            {
                return NotFound();
            }

            return Json(categoriaInsumo);
        }

        // POST: CategoriaInsumos/DeleteCategoriaInsumo/5
        [HttpPost]
        public async Task<IActionResult> DeleteCategoriaInsumo(int id)
        {
            if (_context.CategoriaInsumos == null)
            {
                return Problem("Entity set 'FugazContext.CategoriaInsumos' is null.");
            }

            var categoriaInsumo = await _context.CategoriaInsumos.FindAsync(id);
            if (categoriaInsumo == null)
            {
                return NotFound();
            }

            if (_context.Insumos.Any(i => i.IdCategoria == id))
            {
                return BadRequest("La categoría de insumo no puede ser eliminada porque tiene uno o varios insumos asociados.");
            }

            _context.CategoriaInsumos.Remove(categoriaInsumo);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool CategoriaInsumoExists(int id)
        {
            return (_context.CategoriaInsumos?.Any(e => e.IdCategoria == id)).GetValueOrDefault();
        }
    }
}

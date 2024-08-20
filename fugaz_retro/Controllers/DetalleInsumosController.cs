using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using fugaz_retro.Models;

namespace fugaz_retro.Controllers
{
    public class DetalleInsumosController : Controller
    {
        private readonly FugazContext _context;

        public DetalleInsumosController(FugazContext context)
        {
            _context = context;
        }

        // GET: DetalleInsumos
        public async Task<IActionResult> Index()
        {
            var detalleInsumos = await _context.DetalleInsumos
                .Include(di => di.DetalleProducto)
                .ThenInclude(dp => dp.Producto) // Asegurarse de incluir Producto
                .Include(di => di.Insumo)
                .ToListAsync();
            return View(detalleInsumos);
        }

        // GET: DetalleInsumos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detalleInsumo = await _context.DetalleInsumos
                .Include(di => di.DetalleProducto)
                .Include(di => di.Insumo)
                .FirstOrDefaultAsync(di => di.IdDetalleInsumo == id);

            if (detalleInsumo == null)
            {
                return NotFound();
            }

            return View(detalleInsumo);
        }

        // GET: DetalleInsumos/Create
        public IActionResult Create()
        {
            var detalleProductos = _context.DetalleProductos
                .Include(dp => dp.Producto)
                .Select(dp => new {
                    IdDetalleProducto = dp.IdDetalleProducto,
                    DisplayText = dp.Producto.NombreProducto + " - " + dp.Color
                }).ToList();

            ViewBag.IdDetalleProducto = new SelectList(detalleProductos, "IdDetalleProducto", "DisplayText");
            ViewBag.IdInsumo = new SelectList(_context.Insumos, "IdInsumo", "NombreInsumo");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdDetalleProducto,IdInsumo,Cantidad")] DetalleInsumo detalleInsumo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(detalleInsumo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var detalleProductos = _context.DetalleProductos
                .Include(dp => dp.Producto)
                .Select(dp => new {
                    IdDetalleProducto = dp.IdDetalleProducto,
                    DisplayText = dp.Producto.NombreProducto + " - " + dp.Color
                }).ToList();

            ViewBag.IdDetalleProducto = new SelectList(detalleProductos, "IdDetalleProducto", "DisplayText", detalleInsumo.IdDetalleProducto);
            ViewBag.IdInsumo = new SelectList(_context.Insumos, "IdInsumo", "NombreInsumo", detalleInsumo.IdInsumo);
            return View(detalleInsumo);
        }

        // GET: DetalleInsumos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detalleInsumo = await _context.DetalleInsumos.FindAsync(id);
            if (detalleInsumo == null)
            {
                return NotFound();
            }

            var detalleProductos = _context.DetalleProductos
                .Include(dp => dp.Producto)
                .Select(dp => new {
                    IdDetalleProducto = dp.IdDetalleProducto,
                    DisplayText = dp.Producto.NombreProducto + " - " + dp.Color
                }).ToList();

            ViewBag.IdDetalleProducto = new SelectList(detalleProductos, "IdDetalleProducto", "DisplayText", detalleInsumo.IdDetalleProducto);
            ViewBag.IdInsumo = new SelectList(_context.Insumos, "IdInsumo", "NombreInsumo", detalleInsumo.IdInsumo);
            return View(detalleInsumo);
        }

        // POST: DetalleInsumos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdDetalleInsumo,IdDetalleProducto,IdInsumo,Cantidad")] DetalleInsumo detalleInsumo)
        {
            if (id != detalleInsumo.IdDetalleInsumo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(detalleInsumo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DetalleInsumoExists(detalleInsumo.IdDetalleInsumo))
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

            var detalleProductos = _context.DetalleProductos
                .Include(dp => dp.Producto)
                .Select(dp => new {
                    IdDetalleProducto = dp.IdDetalleProducto,
                    DisplayText = dp.Producto.NombreProducto + " - " + dp.Color
                }).ToList();

            ViewBag.IdDetalleProducto = new SelectList(detalleProductos, "IdDetalleProducto", "DisplayText", detalleInsumo.IdDetalleProducto);
            ViewBag.IdInsumo = new SelectList(_context.Insumos, "IdInsumo", "NombreInsumo", detalleInsumo.IdInsumo);
            return View(detalleInsumo);
        }

        // GET: DetalleInsumos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detalleInsumo = await _context.DetalleInsumos
                .Include(di => di.DetalleProducto)
                .Include(di => di.Insumo)
                .FirstOrDefaultAsync(di => di.IdDetalleInsumo == id);

            if (detalleInsumo == null)
            {
                return NotFound();
            }

            return View(detalleInsumo);
        }

        // POST: DetalleInsumos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var detalleInsumo = await _context.DetalleInsumos.FindAsync(id);
            if (detalleInsumo != null)
            {
                _context.DetalleInsumos.Remove(detalleInsumo);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }


        private bool DetalleInsumoExists(int id)
        {
            return _context.DetalleInsumos.Any(di => di.IdDetalleInsumo == id);
        }
    }
}

using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using fugaz_retro.Models;
using Microsoft.Extensions.Logging;

namespace fugaz_retro.Controllers
{
    public class DetalleComprasController : Controller
    {
        private readonly FugazContext _context;
        private readonly ILogger<DetalleComprasController> _logger;

        // Este es el único constructor del controlador
        public DetalleComprasController(FugazContext context, ILogger<DetalleComprasController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: DetalleCompras
        public async Task<IActionResult> Index()
        {
            var detalleCompras = await _context.DetalleCompras
                .Include(d => d.IdCompraNavigation)
                .Include(d => d.IdInsumoNavigation)
                .ToListAsync();

            return View(detalleCompras);
        }

        // GET: DetalleCompras/Create
        [HttpGet]
        public IActionResult Create()
        {
            ViewData["IdCompra"] = new SelectList(_context.Compras, "IdCompra", "FechaCompra");
            ViewData["IdInsumo"] = new SelectList(_context.Insumos, "IdInsumo", "NombreInsumo");
            return View();
        }

        // POST: DetalleCompras/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DetalleCompra detalleCompra)
        {
            _logger.LogInformation($"Cantidad recibida: {detalleCompra.Cantidad}");

            try
            {
                if (ModelState.IsValid)
                {
                    _context.DetalleCompras.Add(detalleCompra);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear DetalleCompra");
                ModelState.AddModelError("", "Error al guardar los datos.");
            }

            ViewData["IdCompra"] = new SelectList(_context.Compras, "IdCompra", "FechaCompra", detalleCompra.IdCompra);
            ViewData["IdInsumo"] = new SelectList(_context.Insumos, "IdInsumo", "NombreInsumo", detalleCompra.IdInsumo);
            return View(detalleCompra);
        }

        // POST: DetalleCompras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var detalleCompra = await _context.DetalleCompras.FindAsync(id);
            if (detalleCompra == null)
            {
                return NotFound();
            }

            var insumo = await _context.Insumos.FindAsync(detalleCompra.IdInsumo);
            if (insumo != null)
            {
                _context.Insumos.Update(insumo);
            }

            _context.DetalleCompras.Remove(detalleCompra);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DetalleCompraExists(int id)
        {
            return _context.DetalleCompras.Any(e => e.IdDetalleCompra == id);
        }
    }
}

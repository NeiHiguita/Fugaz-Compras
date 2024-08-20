using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using fugaz_retro.Models;
using Microsoft.AspNetCore.Authorization;

namespace fugaz_retro.Controllers
{
    [Authorize]
    public class ProveedorsController : Controller
    {
        private readonly FugazContext _context;

        public ProveedorsController(FugazContext context)
        {
            _context = context;
        }

        // GET: Proveedors
        public async Task<IActionResult> Index()
        {
            return _context.Proveedors != null ?
                        View(await _context.Proveedors.ToListAsync()) :
                        Problem("Entity set 'FugazContext.Proveedors' is null.");
        }

        // GET: Proveedors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Proveedors == null)
            {
                return NotFound();
            }

            var proveedor = await _context.Proveedors
                .FirstOrDefaultAsync(m => m.IdProveedor == id);
            if (proveedor == null)
            {
                return NotFound();
            }

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                // Devuelve los detalles en formato JSON si la solicitud es AJAX
                return Json(proveedor);
            }

            return View(proveedor);
        }



        // GET: Proveedors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Proveedors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdProveedor,TipoProveedor,Empresa,RepresentanteLegal,Rut,NombreCompleto,Documento,Telefono,DireccionAlternativa")] Proveedor proveedor)
        {
            if (ModelState.IsValid)
            {
                // Validar duplicados basados en el tipo de proveedor
                if (proveedor.TipoProveedor == "Natural")
                {
                    // Usamos AnyAsync para verificar duplicados de Documento
                    if (await _context.Proveedors.AnyAsync(p => p.TipoProveedor == "Natural" && p.Documento == proveedor.Documento))
                    {
                        // Si es duplicado, agregamos un mensaje de error al estado del modelo
                        ModelState.AddModelError("Documento", "El documento del proveedor ya está registrado.");
                        return Json(new { success = false, errorMessage = "El documento del proveedor ya está registrado." });
                    }
                }
                else if (proveedor.TipoProveedor == "Jurídico")
                {
                    // Usamos AnyAsync para verificar duplicados de Rut
                    if (await _context.Proveedors.AnyAsync(p => p.TipoProveedor == "Jurídico" && p.Rut == proveedor.Rut))
                    {
                        // Si es duplicado, agregamos un mensaje de error al estado del modelo
                        ModelState.AddModelError("Rut", "El Rut del proveedor ya está registrado.");
                        return Json(new { success = false, errorMessage = "El Rut del proveedor ya está registrado." });
                    }
                }

                // Establecer el estado por defecto como activo
                proveedor.Estado = true;

                try
                {
                    // Agregar y guardar el nuevo proveedor en el contexto
                    _context.Proveedors.Add(proveedor);
                    await _context.SaveChangesAsync();

                    // Devolver una respuesta JSON indicando éxito y la redirección deseada
                    return Json(new { success = true, redirectUrl = Url.Action("Index") });
                }
                catch (Exception ex)
                {
                    // Manejar excepciones y devolver un mensaje de error
                    return Json(new { success = false, errorMessage = "Error al crear el proveedor: " + ex.Message });
                }
            }

            // Devolver una respuesta JSON indicando error si el estado del modelo no es válido
            return Json(new { success = false, errorMessage = "Error al crear el proveedor. Verifique los datos e intente nuevamente." });
        }

        // POST: Proveedors/ToggleEstado/5
        [HttpPost]
        public async Task<IActionResult> ToggleEstado(int id)
        {
            var proveedor = await _context.Proveedors.FindAsync(id);
            if (proveedor == null)
            {
                return NotFound();
            }

            // Cambiar el estado del proveedor y guardar los cambios
            proveedor.Estado = !proveedor.Estado;
            _context.Update(proveedor);
            await _context.SaveChangesAsync();

            // Devolver una respuesta de éxito
            return Ok();
        }

        // GET: Proveedors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Proveedors == null)
            {
                return NotFound();
            }

            var proveedor = await _context.Proveedors.FindAsync(id);
            if (proveedor == null)
            {
                return NotFound();
            }
            return View(proveedor);
        }

        // POST: Proveedors/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdProveedor,TipoProveedor,Empresa,RepresentanteLegal,Rut,NombreCompleto,Documento,Telefono,TipoVia,NumeroVia,Letra1,Letra2,NumeroAdicional,LetraAdicional,Cuadrante,DireccionAlternativa,DireccionGenerada,Estado")] Proveedor proveedor)
        {
            if (id != proveedor.IdProveedor)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(proveedor);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Proveedor editado exitosamente.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProveedorExists(proveedor.IdProveedor))
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
            return View(proveedor);
        }

        // GET: Proveedors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Proveedors == null)
            {
                return NotFound();
            }

            var proveedor = await _context.Proveedors
                .FirstOrDefaultAsync(m => m.IdProveedor == id);
            if (proveedor == null)
            {
                return NotFound();
            }

            return View(proveedor);
        }

        // POST: Proveedors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Proveedors == null)
            {
                return Problem("Entity set 'FugazContext.Proveedors' is null.");
            }

            var proveedor = await _context.Proveedors.Include(p => p.Compras).FirstOrDefaultAsync(p => p.IdProveedor == id);
            if (proveedor == null)
            {
                return NotFound();
            }

            if (proveedor.Compras.Any())
            {
                TempData["ErrorMessage"] = "El proveedor no se puede eliminar ya que se encuentra asociado a una compra.";
                return RedirectToAction(nameof(Index));
            }

            _context.Proveedors.Remove(proveedor);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Proveedor eliminado exitosamente.";
            return RedirectToAction(nameof(Index));
        }

        // GET: Proveedors/GetProveedorDetails/5
        public async Task<IActionResult> GetProveedorDetails(int? id)
        {
            if (id == null || _context.Proveedors == null)
            {
                return NotFound();
            }

            var proveedor = await _context.Proveedors.FirstOrDefaultAsync(m => m.IdProveedor == id);
            if (proveedor == null)
            {
                return NotFound();
            }

            return Json(proveedor);
        }

        // POST: Proveedors/DeleteProveedor/5
        [HttpPost]
        public async Task<IActionResult> DeleteProveedor(int id)
        {
            if (_context.Proveedors == null)
            {
                return Problem("Entity set 'FugazContext.Proveedors' is null.");
            }

            var proveedor = await _context.Proveedors.FindAsync(id);
            if (proveedor == null)
            {
                return NotFound();
            }

            if (_context.Compras.Any(c => c.IdProveedor == id))
            {
                return BadRequest("El proveedor no puede ser eliminado porque tiene una o varias compras asociadas.");
            }

            _context.Proveedors.Remove(proveedor);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // Acción para obtener los detalles del proveedor
        public IActionResult GetProveedorDetails(int id)
        {
            var proveedor = _context.Proveedors
                .Where(p => p.IdProveedor == id)
                .Select(p => new
                {
                    p.TipoProveedor,
                    p.Empresa,
                    p.RepresentanteLegal,
                    p.Rut,
                    p.NombreCompleto,
                    p.Documento,
                    p.Telefono,
                    p.DireccionAlternativa,
                    p.Estado
                })
                .FirstOrDefault();

            if (proveedor == null)
            {
                return NotFound("Proveedor no encontrado");
            }

            return Json(proveedor);
        }

        private bool ProveedorExists(int id)
        {
            return (_context.Proveedors?.Any(e => e.IdProveedor == id)).GetValueOrDefault();
        }
    }
}

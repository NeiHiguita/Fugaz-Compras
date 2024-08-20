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
    public class RolPermisosController : Controller
    {
        private readonly FugazContext _context;

        public RolPermisosController(FugazContext context)
        {
            _context = context;
        }

        // GET: RolPermisos
        public async Task<IActionResult> Index()
        {
            var fugazContext = _context.RolPermisos.Include(r => r.IdPermisoNavigation).Include(r => r.IdRolNavigation);
            return View(await fugazContext.ToListAsync());
        }

        // GET: RolPermisos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.RolPermisos == null)
            {
                return NotFound();
            }

            var rolPermiso = await _context.RolPermisos
                .Include(r => r.IdPermisoNavigation)
                .Include(r => r.IdRolNavigation) // Incluir la navegación al objeto de rol
                    .ThenInclude(r => r.RolPermisos) // Incluir la navegación a los permisos asociados al rol
                        .ThenInclude(rp => rp.IdPermisoNavigation) // Incluir la navegación al objeto de permiso
                .FirstOrDefaultAsync(m => m.IdRolPermiso == id);

            if (rolPermiso == null)
            {
                return NotFound();
            }

            return View(rolPermiso);
        }

        // GET: RolPermisos/Create
        public IActionResult Create()
        {
            ViewData["IdRol"] = new SelectList(_context.Roles, "IdRol", "NombreRol");
            ViewData["Permisos"] = _context.Permisos.ToList(); // Obtener todos los permisos disponibles

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int idRol, List<int> permisosSeleccionados)
        {
            if (ModelState.IsValid)
            {
                // Primero, eliminamos las asociaciones existentes para el rol
                var rolPermisosExistentes = _context.RolPermisos.Where(rp => rp.IdRol == idRol);
                _context.RolPermisos.RemoveRange(rolPermisosExistentes);
                await _context.SaveChangesAsync();

                // Luego, agregamos las nuevas asociaciones
                foreach (var idPermiso in permisosSeleccionados)
                {
                    var rolPermiso = new RolPermiso
                    {
                        IdRol = idRol,
                        IdPermiso = idPermiso
                    };
                    _context.RolPermisos.Add(rolPermiso);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdRol"] = new SelectList(_context.Roles, "IdRol", "NombreRol", idRol);
            ViewData["Permisos"] = _context.Permisos.ToList();
            return View();
        }


        // GET: RolPermisos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.RolPermisos == null)
            {
                return NotFound();
            }

            var rolPermiso = await _context.RolPermisos.FindAsync(id);
            if (rolPermiso == null)
            {
                return NotFound();
            }
            ViewData["IdPermiso"] = new SelectList(_context.Permisos, "IdPermiso", "IdPermiso", rolPermiso.IdPermiso);
            ViewData["IdRol"] = new SelectList(_context.Roles, "IdRol", "IdRol", rolPermiso.IdRol);
            return View(rolPermiso);
        }

        // POST: RolPermisos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdRolPermiso,IdRol,IdPermiso")] RolPermiso rolPermiso)
        {
            if (id != rolPermiso.IdRolPermiso)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rolPermiso);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RolPermisoExists(rolPermiso.IdRolPermiso))
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
            ViewData["IdPermiso"] = new SelectList(_context.Permisos, "IdPermiso", "IdPermiso", rolPermiso.IdPermiso);
            ViewData["IdRol"] = new SelectList(_context.Roles, "IdRol", "IdRol", rolPermiso.IdRol);
            return View(rolPermiso);
        }

        // GET: RolPermisos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.RolPermisos == null)
            {
                return NotFound();
            }

            var rolPermiso = await _context.RolPermisos
                .Include(r => r.IdPermisoNavigation)
                .Include(r => r.IdRolNavigation)
                .FirstOrDefaultAsync(m => m.IdRolPermiso == id);
            if (rolPermiso == null)
            {
                return NotFound();
            }

            return View(rolPermiso);
        }

        // POST: RolPermisos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.RolPermisos == null)
            {
                return Problem("Entity set 'FugazContext.RolPermisos'  is null.");
            }
            var rolPermiso = await _context.RolPermisos.FindAsync(id);
            if (rolPermiso != null)
            {
                _context.RolPermisos.Remove(rolPermiso);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RolPermisoExists(int id)
        {
            return (_context.RolPermisos?.Any(e => e.IdRolPermiso == id)).GetValueOrDefault();
        }
    }
}
using fugaz_retro.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fugaz_retro.Controllers
{
    public class CostoEnvioController : Controller
    {
        private readonly FugazContext _context;

        public CostoEnvioController(FugazContext context)
        {
            _context = context;
        }

        // Método para obtener los departamentos
        private List<SelectListItem> GetDepartamentos()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Value = "Antioquia", Text = "Antioquia" },
                new SelectListItem { Value = "Cundinamarca", Text = "Cundinamarca" },
                new SelectListItem { Value = "Valle del Cauca", Text = "Valle del Cauca" },
                new SelectListItem { Value = "Bolívar", Text = "Bolívar" },
                new SelectListItem { Value = "Atlántico", Text = "Atlántico" }
            };
        }

        // Método para obtener las ciudades basadas en el departamento
        private Dictionary<string, List<SelectListItem>> GetCiudades()
        {
            return new Dictionary<string, List<SelectListItem>>
            {
                { "Antioquia", new List<SelectListItem> { new SelectListItem { Value = "Medellin", Text = "Medellín" } } },
                { "Cundinamarca", new List<SelectListItem> { new SelectListItem { Value = "Bogota", Text = "Bogotá" } } },
                { "Valle del Cauca", new List<SelectListItem> { new SelectListItem { Value = "Cali", Text = "Cali" } } },
                { "Bolívar", new List<SelectListItem> { new SelectListItem { Value = "Cartagena", Text = "Cartagena" } } },
                { "Atlántico", new List<SelectListItem> { new SelectListItem { Value = "Barranquilla", Text = "Barranquilla" } } }
            };
        }

        // Acción para obtener las ciudades basadas en el departamento seleccionado
        public JsonResult GetCiudades(string departamento)
        {
            var ciudades = GetCiudades().ContainsKey(departamento) ? GetCiudades()[departamento] : new List<SelectListItem>();
            return Json(ciudades);
        }

        // Acción Index
        public async Task<IActionResult> Index()
        {
            return View(await _context.CostoEnvios.ToListAsync());
        }

        // Acción Create GET
        public IActionResult Create()
        {
            ViewData["Departamentos"] = new SelectList(GetDepartamentos(), "Value", "Text");
            ViewData["Ciudades"] = new SelectList(Enumerable.Empty<SelectListItem>(), "Value", "Text");
            return View();
        }

        // Acción Create POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CostoEnvio costoEnvio)
        {
            if (ModelState.IsValid)
            {
                _context.Add(costoEnvio);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Departamentos"] = new SelectList(GetDepartamentos(), "Value", "Text");
            return View(costoEnvio);
        }

    
        // Acción Edit GET
        public async Task<IActionResult> Edit(int id)
        {
            var costoEnvio = await _context.CostoEnvios.FindAsync(id);
            if (costoEnvio == null)
            {
                return NotFound();
            }
            ViewData["Departamentos"] = new SelectList(GetDepartamentos(), "Value", "Text");
            ViewData["Ciudades"] = new SelectList(GetCiudades()[costoEnvio.Departamento], "Value", "Text");
            return View(costoEnvio);
        }

        // Acción Edit POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CostoEnvio costoEnvio)
        {
            if (id != costoEnvio.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(costoEnvio);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CostoEnvioExists(costoEnvio.Id))
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
            ViewData["Departamentos"] = new SelectList(GetDepartamentos(), "Value", "Text");
            return View(costoEnvio);
        }

        // Acción Delete GET
        public async Task<IActionResult> Delete(int id)
        {
            var costoEnvio = await _context.CostoEnvios.FindAsync(id);
            if (costoEnvio == null)
            {
                return NotFound();
            }
            return View(costoEnvio);
        }

        // Acción Delete POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var costoEnvio = await _context.CostoEnvios.FindAsync(id);
            _context.CostoEnvios.Remove(costoEnvio);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Método para verificar si un costo de envío existe
        private bool CostoEnvioExists(int id)
        {
            return _context.CostoEnvios.Any(e => e.Id == id);
        }
    }
}

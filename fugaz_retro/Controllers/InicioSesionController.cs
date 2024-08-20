using Microsoft.AspNetCore.Mvc;
using fugaz_retro.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;


namespace fugaz_retro.Controllers
{
    [Authorize]

    public class InicioSesionController : Controller
    {
        private readonly FugazContext _context;

        public InicioSesionController(FugazContext context)
        {
            _context = context;
        }

        public IActionResult IniciarSesion()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> IniciarSesion(string correo, string contraseña)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Correo == correo && u.Contraseña == contraseña);

            if (usuario != null)
            {
                // Iniciar sesión
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Correo o contraseña incorrectos");
            return View();
        }
    }

}

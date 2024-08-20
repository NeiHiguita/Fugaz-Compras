using fugaz_retro.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace fugaz_retro.Controllers
{
    public class RegistroController : Controller
    {
        private readonly FugazContext _context;

        public RegistroController(FugazContext context)
        {
            _context = context;
        }

        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registro(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction("Login");
            }
            return View(usuario);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string Usuario, string Contraseña)
        {
            // Verificar las credenciales del usuario y realizar el inicio de sesión

            // Ejemplo de verificación básica (¡no recomendado para producción!)
            if (Usuario == "usuario" && Contraseña == "contraseña")
            {
                // Si las credenciales son válidas, inicia sesión
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, Usuario),
                    // Agrega cualquier otro claim que desees, como roles, etc.
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    // Opcional: Puedes configurar propiedades como IsPersistent para la cookie
                    IsPersistent = true
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                // Redirigir al usuario a una página después del inicio de sesión
                return RedirectToAction("Index", "Home");
            }

            // Si las credenciales no son válidas, redirigir de nuevo al login con un mensaje de error
            TempData["ErrorMessage"] = "Credenciales inválidas";
            return RedirectToAction("Login");
        }
    }
}

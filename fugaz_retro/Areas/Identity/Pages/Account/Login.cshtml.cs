using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using fugaz_retro.Models;
using Microsoft.AspNetCore.Http;

namespace fugaz_retro.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly FugazContext _context;

        public LoginModel(SignInManager<IdentityUser> signInManager, ILogger<LoginModel> logger, FugazContext context)
        {
            _signInManager = signInManager;
            _context = context;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        [TempData]
        public bool InactiveUser { get; set; }  // TempData for inactive user

        public class InputModel
        {
            [Required(ErrorMessage = "El campo Correo es obligatorio.")]
            [EmailAddress(ErrorMessage = "El campo Correo no es una dirección de correo electrónico válida.")]
            public string Email { get; set; }

            [Required(ErrorMessage = "El campo Contraseña es obligatorio.")]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Recuérdame")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("/Principal");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                var user = await _signInManager.UserManager.FindByEmailAsync(Input.Email);
                if (user != null)
                {
                    if (!user.EmailConfirmed)
                    {
                        InactiveUser = true;
                        return RedirectToPage();
                    }

                    var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User logged in.");

                        var usuario = await _context.Usuarios.Include(u => u.IdRolNavigation)
                                      .ThenInclude(r => r.RolPermisos)
                                      .ThenInclude(rp => rp.IdPermisoNavigation)
                                      .FirstOrDefaultAsync(u => u.Correo == Input.Email);

                        if (usuario != null)
                        {
                            var IdRol = usuario.IdRol;
                            var doc = usuario.Document;
                            HttpContext.Session.SetInt32("IdRol", (int)IdRol);

                            // Obtener los permisos del usuario y almacenarlos en la sesión
                            var permisos = usuario.IdRolNavigation.RolPermisos.Select(rp => rp.IdPermisoNavigation.NombrePermiso).ToList();
                            HttpContext.Session.SetString("Permisos", string.Join(",", permisos));

                            if (IdRol == 7 && doc != null)
                            {
                                return Redirect("/Home/index");
                            }
                            else if(IdRol == 7 && doc == null)
                            {
                                return Redirect("/Home/Miperfil");
                            }

                            return LocalRedirect(returnUrl);
                        }
                    }
                    if (result.RequiresTwoFactor)
                    {
                        return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                    }
                    if (result.IsLockedOut)
                    {
                        _logger.LogWarning("User account locked out.");
                        return RedirectToPage("./Lockout");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Usuario o contraseña está incorrecto.");
                        return Page();
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Usuario o contraseña está incorrecto.");
                    return Page();
                }
            }
            return Page();
        }
    }
}

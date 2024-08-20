using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace fugaz_retro.Controllers
{
    public class ApkController : Controller
    {
        public IActionResult DescargarAPK()
        {
            // Ruta al archivo APK que quieres descargar
            string rutaAPK = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "AppMovil", "geometry-dash-apk-v2.2.13.apk");

            // Verifica si el archivo existe
            if (!System.IO.File.Exists(rutaAPK))
            {
                return NotFound();
            }

            // Nombre del archivo APK que se enviará al navegador
            string nombreAPK = "FugazRt.apk";

            // Envía el archivo APK al navegador para descargar
            return PhysicalFile(rutaAPK, "application/vnd.android.package-archive", nombreAPK);
        }
    }
}

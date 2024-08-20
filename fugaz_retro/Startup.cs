using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
namespace fugaz_retro
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // Este método se llama en tiempo de ejecución. Utilice este método para agregar servicios al contenedor.
        public void ConfigureServices(IServiceCollection services)
        {
            // Otros servicios

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(560); // Tiempo de expiración de la sesión
                options.Cookie.HttpOnly = true; // La cookie de sesión solo puede ser accedida por el servidor
                options.Cookie.IsEssential = true; // Esencial para el funcionamiento de la aplicación
            });

            // Otros servicios
        }

        // Este método se llama en tiempo de ejecución. Utilice este método para configurar el pipeline de solicitud HTTP.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Otro middleware

            app.UseSession();

            // Otro middleware

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

        }

    }
}

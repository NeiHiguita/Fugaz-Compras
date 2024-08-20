using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using fugaz_retro.Models;
using System;

namespace fugaz_retro.Controllers
{
    public class PrincipalController : Controller
    {
        private readonly FugazContext _context;

        public PrincipalController(FugazContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? month, int? year)
        {
            // Obtener las ventas con las navegaciones necesarias
            var ventas = await _context.Ventas
                .Include(v => v.IdPedidoNavigation)
                .ThenInclude(p => p.IdClienteNavigation)
                .ThenInclude(c => c.Usuarios)
                .ToListAsync();

            // Verificar si se han obtenido ventas
            if (ventas == null || !ventas.Any())
            {
                ViewBag.Message = "No se encontraron ventas.";
            }

            // Filtrar por mes y año si se proporcionan
            if (month.HasValue && year.HasValue)
            {
                ventas = ventas.Where(v => v.IdPedidoNavigation.FechaPedido.Year == year.Value && v.IdPedidoNavigation.FechaPedido.Month == month.Value).ToList();
            }

            // Agrupar las ventas por cliente y obtener los 5 principales
            var ventasPorCliente = ventas
                .Where(v => v.IdPedidoNavigation?.IdClienteNavigation?.Usuarios != null)
                .GroupBy(v => v.IdPedidoNavigation.IdClienteNavigation.Usuarios.NombreUsuario)
                .Select(group => new
                {
                    Cliente = group.Key,
                    Count = group.Count()
                })
                .OrderByDescending(v => v.Count)
                .Take(5)
                .ToList();

            // Agrupar las ventas por método de pago
            var ventasPorMetodoPago = ventas
                .GroupBy(v => v.IdPedidoNavigation.MetodoPago)
                .Select(g => new { MetodoPago = g.Key, Count = g.Count() })
                .ToList();

            // Agrupar las ventas por ciudad y obtener al menos 5 ciudades
            var ventasPorCiudad = ventas
                .GroupBy(v => v.IdPedidoNavigation.Ciudad)
                .Select(g => new { Ciudad = g.Key, Count = g.Count() })
                .OrderByDescending(g => g.Count)
                .Take(5)
                .ToList();

            // Agrupar las ventas por mes
            var ventasPorMes = ventas
                .Where(v => v.IdPedidoNavigation != null && v.IdPedidoNavigation.FechaPedido != null) // Verifica que la fecha no sea nula
                .GroupBy(v => new { v.IdPedidoNavigation.FechaPedido.Year, v.IdPedidoNavigation.FechaPedido.Month })
                .Select(g => new { Mes = new DateTime(g.Key.Year, g.Key.Month, 1), Count = g.Count() })
                .OrderBy(g => g.Mes)
                .ToList();

            // Obtener los pedidos anulados
            var pedidos = await _context.Pedidos.ToListAsync();
            var pedidosAnulados = pedidos.Where(p => p.Estado == "Anulado").ToList();

            // Pasar los datos a la vista
            ViewBag.VentasPorCliente = ventasPorCliente;
            ViewBag.VentasPorMetodoPago = ventasPorMetodoPago;
            ViewBag.VentasPorCiudad = ventasPorCiudad;
            ViewBag.VentasPorMes = ventasPorMes;
            ViewBag.Pedidos = pedidos;
            ViewBag.PedidosAnulados = pedidosAnulados;

            return View();
        }
    }
}

using System;
using System.Collections.Generic;

namespace fugaz_retro.Models
{
    public partial class Pedido
    {
        public int IdPedido { get; set; }
        public int? IdCliente { get; set; }
        public string? Estado { get; set; }
        public string MetodoPago { get; set; } = null!;
        public string? TipoTransferencia { get; set; }
        public DateTime FechaPedido { get; set; }
        public DateTime FechaEntrega { get; set; }
        public double CostoPedido { get; set; }
        public double CostoEnvio { get; set; }
        public double Descuento { get; set; }
        public double TotalPedido { get; set; }
        public byte[]? ComprobantePago { get; set; }
        public string? Direccion { get; set; }
        public string? Ciudad { get; set; }
        public virtual ICollection<DetallePedido> DetallePedidos { get; } = new List<DetallePedido>();
        public virtual Cliente? IdClienteNavigation { get; set; }
        public virtual ICollection<Venta> Venta { get; } = new List<Venta>();
    }
}

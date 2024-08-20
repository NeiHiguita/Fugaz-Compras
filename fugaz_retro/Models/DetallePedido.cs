using System;
using System.Collections.Generic;

namespace fugaz_retro.Models;

public class DetallePedido
{
    public int IdDetallePedido { get; set; }
    public int IdPedido { get; set; }
    public int IdDetalleProducto { get; set; }
    public int Cantidad { get; set; }
    public string? talla { get; set; } 
    public string? color { get; set; }
    public double Subtotal { get; set; }

    public Pedido? IdPedidoNavigation { get; set; }
    public DetalleProducto? IdDetalleProductoNavigation { get; set; }
    //public Producto? Producto { get; set; }
}

using System;
using System.Collections.Generic;

namespace fugaz_retro.Models;

public partial class Venta
{
    public int IdVenta { get; set; }
    public int? IdPedido { get; set; }
    public virtual Pedido? IdPedidoNavigation { get; set; }

}


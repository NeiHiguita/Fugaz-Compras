using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
namespace fugaz_retro.Models;

public partial class DetalleCompra
{
    public int IdDetalleCompra { get; set; }

    public int? IdCompra { get; set; }
    public int? IdInsumo { get; set; }
    public decimal Cantidad { get; set; }
    public string UnidadMedida { get; set; } = null!;
    public double PrecioUnitario { get; set; }
    public virtual Compra? IdCompraNavigation { get; set; }
    public virtual Insumo? IdInsumoNavigation { get; set; }
    //public void SetCantidad(string cantidad)
    //{
    //    Cantidad = Convert.ToDouble(cantidad, CultureInfo.InvariantCulture);
    //}
}
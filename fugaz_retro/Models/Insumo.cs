using System;
using System.Collections.Generic;
namespace fugaz_retro.Models;
public partial class Insumo
{
    public int IdInsumo { get; set; }
    public int? IdCategoria { get; set; }
    public string NombreInsumo { get; set; } = null!;
    public string UnidadMedida { get; set; } = null!;
    public string CategoriaInsumo { get; set; } = null!;
    public int? Cantidad { get; set; }
    public string? Descripcion { get; set; }
    public double Stock { get; set; }
    public string? Estado { get; set; }
    public double PrecioUnitario { get; set; }
    public virtual ICollection<DetalleCompra> DetalleCompras { get; } = new List<DetalleCompra>();
    public virtual ICollection<PerdidaInsumo> PerdidaInsumos { get; } = new List<PerdidaInsumo>();
    public virtual CategoriaInsumo? IdCategoriaNavigation { get; set; }
}
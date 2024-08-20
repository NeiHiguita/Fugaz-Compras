using System;
using System.Collections.Generic;

namespace fugaz_retro.Models;

public partial class PerdidaInsumo
{
    public int IdPerdidaInsumo { get; set; }

    public string? TipoInsumo { get; set; }

    public int? IdInsumo { get; set; }

    public decimal Cantidad { get; set; }

    public string? UnidadMedida { get; set; } = null!;

    public DateTime Fecha { get; set; }

    public virtual Insumo? IdInsumoNavigation { get; set; }
}
using System;
using System.Collections.Generic;

namespace fugaz_retro.Models;

public partial class CategoriaInsumo
{
    public int IdCategoria { get; set; }

    public string NombreCategoria { get; set; } = null!;

    public bool EstadoCategoria { get; set; }

    public virtual ICollection<Insumo> Insumos { get; } = new List<Insumo>();
}

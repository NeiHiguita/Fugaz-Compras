using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace fugaz_retro.Models;

public partial class Proveedor
{
    public int IdProveedor { get; set; }

    public string TipoProveedor { get; set; } = null!;

    public string? Empresa { get; set; } = null!;

    public string? RepresentanteLegal { get; set; } = null!;
    public int? Rut { get; set; }

    public string? NombreCompleto { get; set; }
    public string? Documento { get; set; }

     public string Telefono { get; set; } = null!;
    public string DireccionAlternativa { get; set; }

    public bool Estado { get; set; }

    public virtual ICollection<Compra> Compras { get; } = new List<Compra>();

    // Validación personalizada para evitar duplicados de documento o RUT
    public bool IsValid()
    {
        using (var context = new FugazContext())
        {
            // Verificar si ya existe otro proveedor con el mismo documento o RUT
            return !context.Proveedors.Any(p => p.IdProveedor != this.IdProveedor && (p.Documento == this.Documento || p.Rut == this.Rut));
        }
    }
}



using System;
using System.Collections.Generic;

namespace fugaz_retro.Models
{
    public partial class Producto
    {
        public Producto()
        {
            DetalleProductos = new HashSet<DetalleProducto>();
        }
        public int IdProducto { get; set; }
        public string NombreProducto { get; set; } = null!;
        public double PrecioVenta { get; set; }
        public string CategoriaProducto { get; set; } = null!;
        public byte[]? Foto { get; set; }
        public string Estado { get; set; }
        public virtual ICollection<DetalleProducto> DetalleProductos { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace fugaz_retro.Models
{
    public class DetalleProducto
    {
        public DetalleProducto()
        {
            DetalleInsumos = new HashSet<DetalleInsumo>();
        }
        public int IdDetalleProducto { get; set; }
        public int IdProducto { get; set; }
        public string? Talla { get; set; }
        public string? Color { get; set; }
        public Producto? Producto { get; set; } // Relación con el producto
        public virtual ICollection<DetalleInsumo> DetalleInsumos { get; set; }

        public virtual ICollection<DetallePedido>? DetallePedidos { get; set; }
    }

}

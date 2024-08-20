using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;


namespace fugaz_retro.Models;

public class ProductoViewModel
{
    public List<Producto> Productos { get; set; }
    public List<DetalleProducto> DetalleProductos { get; set; }
}
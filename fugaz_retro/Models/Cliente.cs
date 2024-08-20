using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;


namespace fugaz_retro.Models;

public partial class Cliente
{
    public int IdCliente { get; set; }
    public int IdUsuario { get; set; }
    public string? Telefono { get; set; }

    [ForeignKey("IdUsuario")]
    public virtual Usuario? Usuarios { get; set; }

    public virtual ICollection<Pedido>? Pedidos { get; set; }

}

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace fugaz_retro.Models
{
    public partial class Usuario
    {
        public int IdUsuario { get; set; }
        public int? IdRol { get; set; }

        [Required(ErrorMessage = "El campo Nombre de usuario es obligatorio.")]
        public string NombreUsuario { get; set; } = null!;

        [Required(ErrorMessage = "El campo Correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "El campo Correo no es una dirección de correo electrónico válida.")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.(com|org|net|edu)$", ErrorMessage = "El correo debe tener un dominio válido como @gmail.com, @hotmail.com, etc.")]
        public string Correo { get; set; } = null!;

        [Required(ErrorMessage = "El campo contraseña es obligatorio.")]

        public string Contraseña { get; set; } = null!;
        public bool Estado { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime DateRegister { get; set; }
        public string? Document { get; set; }

        public virtual Role? IdRolNavigation { get; set; }
        public virtual Cliente? Cliente { get; set; }
        public string? RecoveryCode { get; set; }
    }
}
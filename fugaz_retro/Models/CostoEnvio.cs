using System.ComponentModel.DataAnnotations;

namespace fugaz_retro.Models
{
    public class CostoEnvio
    {
        public int Id { get; set; }

        [Required]
        public string Ciudad { get; set; }

        [Required]
        public string Departamento { get; set; }

        public decimal Costo { get; set; }
    }
}

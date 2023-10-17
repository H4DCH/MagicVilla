using System.ComponentModel.DataAnnotations;

namespace MagicVilla_Web
{
    public class NumeroVillaCreateDTO
    {
        [Required]
        public int VillaNo { get; set; }

        [Required]
        public int VillaId { get; set; }
        public string DetalleEspecial { get; set; }

    }
}

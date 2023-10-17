using System.ComponentModel.DataAnnotations;

namespace MagicVilla.Models.DTO
{
    public class VillaCreateDTO
    { 

        [Required(ErrorMessage ="Debe Ingresar un nombre"),MaxLength(30)]
        public string Nombre { get; set; } = string.Empty;
        public string Detalle { get; set; }

        [Required(ErrorMessage = "Debe Ingresar una Tarifa")]
        public double Tarifa { get; set; }  
        public int Ocupantes { get; set; }  
        public int MetrosCuadrados { get; set; }
        public string ImaUrl { get; set; }
        public string Amenidad { get; set; }
    }
}

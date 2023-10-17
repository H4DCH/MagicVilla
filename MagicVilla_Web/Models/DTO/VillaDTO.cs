using System.ComponentModel.DataAnnotations;

namespace MagicVilla_Web.Models.DTO
{
    public class VillaDTO 
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Debe ingresar un nombre"),MaxLength(30)]
        public string Nombre { get; set; } = string.Empty;
        public string Detalle { get; set; }

        [Required(ErrorMessage = "Debe ingresar una tarifa")]
        public double Tarifa { get; set; }  
        public int Ocupantes { get; set; }  
        public int MetrosCuadrados { get; set; }
        public string ImaUrl { get; set; }
        public string Amenidad { get; set; }
    }
}

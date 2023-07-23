using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MagicVilla.Models
{
    public class Villa
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required(ErrorMessage ="Debe Ingresar un Nombre")]
        public string  Nombre { get; set; } = string.Empty;
        public string Detalle { get; set; }
        [Required(ErrorMessage = "Debe Ingresar una Tarifa")]
        public double Tarifa { get; set; }  
        public int Ocupantes { get; set; }  
        public int MetrosCuadrados { get; set; }    
        public string  ImaUrl { get; set; } 
        public string Amenidad { get; set; }   
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }    
       
    }
}

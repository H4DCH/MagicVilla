using MagicVilla_API.Modelos.DTO;

namespace MagicVilla_API.Datos
{
    public static class VillaStore
    {
        public static List<VillaDTO> villaList = new List<VillaDTO> { 
        
            new VillaDTO
            {
                Id=1,
                Nombre="Villa Prueba",
                Ocupantes=3,
                MetrosCuadrados = 50
            },
            new VillaDTO
            {
                Id=2,
                Nombre="Villa Prueba2",
                Ocupantes=2,
                MetrosCuadrados =50 
            }
        
        };


    }
}

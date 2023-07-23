using MagicVilla.Models.DTO;
using System.ComponentModel;

namespace MagicVilla.VillaStore
{
    public static class VillaStore
    {
        public static List<VillaDTO> Villalista = new List<VillaDTO>{ 
            new  VillaDTO {Id = 1, Nombre = "Vista Piscina",Ocupantes = 5, MetrosCuadrados = 50},
            new VillaDTO {Id = 2,Nombre = "Vista Playa", Ocupantes = 4, MetrosCuadrados=40}
            };  

    }
}

using AutoMapper;
using MagicVilla.Models;
using MagicVilla.Models.DTO;

namespace MagicVilla
{
    public class Mapping : Profile
    {
       public Mapping() 
        {
            CreateMap<Villa, VillaDTO>();
            CreateMap<VillaDTO, Villa>();

            CreateMap<Villa, VillaCreateDTO>().ReverseMap();
            CreateMap<Villa, VillaUpdateDTO>().ReverseMap();

            CreateMap<NumeroVilla, NumeroVillaCreateDTO>().ReverseMap();
            CreateMap<NumeroVilla, NumeroVillaUpdateDTO>().ReverseMap();
            CreateMap<NumeroVilla, NumeroVillaDTO>().ReverseMap();
        
        } 
    }
}

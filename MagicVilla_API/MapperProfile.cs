using AutoMapper;
using MagicVilla_API.Modelos;
using MagicVilla_API.Modelos.DTO;

namespace MagicVilla_API
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Villa, VillaDTO>().ReverseMap();
            CreateMap<VillaUpdateDTO, Villa>().ReverseMap();
            CreateMap<VillaCreateDTO, Villa>().ReverseMap(); 
        }
    }
}

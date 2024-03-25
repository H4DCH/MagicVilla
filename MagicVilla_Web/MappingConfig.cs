using AutoMapper;
using MagicVilla_Web.Models.DTO;

namespace MagicVilla_Web
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
           CreateMap<VillaDTO,VillaCreateDTO>().ReverseMap();
            CreateMap<VillaDTO, VillaUpdateDTO>().ReverseMap();

            CreateMap<NumeroVillaDTO, NumeroVillaCreateDTO>().ReverseMap();
            CreateMap<NumeroVillaDTO, NumeroVillaUpdateDTO>().ReverseMap();
        }
    }
}

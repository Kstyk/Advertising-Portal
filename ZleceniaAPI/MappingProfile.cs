using AutoMapper;
using ZleceniaAPI.Entities;
using ZleceniaAPI.Models;

namespace ZleceniaAPI
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Category, CategoryDto>()
                  .ForMember(dest => dest.ChildCategories, opt => opt.MapFrom(src => src.ChildCategories));


            CreateMap<CreateUserCategoryDto, AreaOfWork>()
                .ForMember(r => r.Voivodeship, opt => opt.MapFrom(src => src.Voivodeship))
                .ForMember(r => r.WholeCountry, opt => opt.MapFrom(src => src.WholeCountry));
        }
    }
}

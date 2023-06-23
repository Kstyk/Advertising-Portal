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

            CreateMap<RegisterUserDto, User>()
                .ForMember(r => r.Address, c => c.MapFrom(dto => new Address()
                {
                    Voivodeship = dto.Voivodeship,
                    City = dto.City,
                    Street = dto.Street,
                    PostalCode = dto.PostalCode,
                    BuildingNumber = dto.BuildingNumber
                }));

            //CreateMap<CreateOrderDto, Address>()
            //    .ForMember(r => r.Voivodeship, opt => opt.MapFrom(src => src.Voivodeship))
            //    .ForMember(r => r.City, opt => opt.MapFrom(src => src.City))
            //    .ForMember(r => r.PostalCode, opt => opt.MapFrom(src => src.PostalCode))
            //    .ForMember(r => r.Street, opt => opt.MapFrom(src => src.Street))
            //    .ForMember(r => r.BuildingNumber, opt => opt.MapFrom(src => src.BuildingNumber));
        }
    }
}

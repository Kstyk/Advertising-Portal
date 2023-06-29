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

            CreateMap<AddOrderDto, Order>()
                .ForMember(r => r.Address, c => c.MapFrom(dto => new Address()
                {
                    Voivodeship = dto.Voivodeship,
                    City = dto.City,
                    Street = dto.Street,
                    PostalCode = dto.PostalCode,
                    BuildingNumber = dto.BuildingNumber
                }));

            CreateMap<AddOfferDto, Offer>();

            CreateMap<Order, OrderDto>();
            CreateMap<Offer, OfferDto>()
                .ForMember(r => r.FirstName, c => c.MapFrom(s => s.User.FirstName))
                .ForMember(r => r.LastName, c => c.MapFrom(s => s.User.LastName))
                .ForMember(r => r.Email, c => c.MapFrom(s => s.User.Email))
                .ForMember(r => r.PhoneNumber, c => c.MapFrom(s => s.User.PhoneNumber))
                .ForMember(r => r.CompanyName, c => c.MapFrom(s => s.User.CompanyName));


            CreateMap<Address, AddressDto>();
            CreateMap<User, UserDto>();
            CreateMap<StatusOfUser, StatusOfUserDto>();
            CreateMap<TypeOfAccount, TypeOfAccountDto>();
        }
    }
}

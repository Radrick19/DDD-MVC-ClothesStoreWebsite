using AutoMapper;
using Store.Application.Dto.Account;
using Store.Application.Dto.Administration;
using Store.Application.Dto.Product;
using Store.Domain.Models;
using Store.Domain.Models.ProductEntities;

namespace Store.Application.AutoMapper
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Product, ProductDetailsDto>()
                .ForMember(dest=> dest.CanReturn, opt=> opt.MapFrom(src=> src.Subcategory.CanReturn));
            CreateMap<Product, ProductCatalogDto>();
            CreateMap<ProductDto, ProductCatalogDto>();
            CreateMap<ProductDto, ProductDetailsDto>()
                .ForMember(dest => dest.CanReturn, opt => opt.MapFrom(src => src.Subcategory.CanReturn));
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Subcategory, SubcategoryDto>().ReverseMap();
            CreateMap<ClothingCollection, CollectionDto>().ReverseMap();
            CreateMap<Color, ColorDto>().ReverseMap();
            CreateMap<Size, SizeDto>();
            CreateMap<PromoPage, PromoPageDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
        }
    }
}

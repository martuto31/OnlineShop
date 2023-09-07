using AutoMapper;
using OnlineShop.Mapping.Models;
using OnlineShop.Models;

namespace OnlineShop.Mapping
{
    public class ProductMapping : Profile
    {
        public ProductMapping()
        {
            CreateMap<Product, ProductResponseDTO>()
                .ForMember(dest => dest.ProductSizes, opt => opt.MapFrom(src => src.ProductsWithSizes.Select(p => p.ProductSizes.Size)))
                .ForMember(dest => dest.ProductColors, opt => opt.MapFrom(src => src.ProductsWithColors.Select(p => p.ProductColors.Color)))
                .ForMember(dest => dest.PicturesData, opt => opt.MapFrom(src => src.Pictures.Select(p => Convert.ToBase64String(p.Image))))
                .ReverseMap();
        }
    }
}

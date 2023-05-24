using AutoMapper;
using OnlineShop.Mapping.Models;
using OnlineShop.Models;

namespace OnlineShop.Mapping
{
    public class ProductColorsMapping : Profile
    {
        public ProductColorsMapping() 
        {
            CreateMap<ProductColors, ProductColorsResponseDTO>();
        }
    }
}

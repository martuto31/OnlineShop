using AutoMapper;
using OnlineShop.Mapping.Models;
using OnlineShop.Models;

namespace OnlineShop.Mapping
{
    public class ProductSizesMapping : Profile
    {
        public ProductSizesMapping() 
        { 
            CreateMap<ProductSizes, ProductSizesResponseDTO>();
        }
    }
}

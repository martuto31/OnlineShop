using AutoMapper;
using OnlineShop.Mapping.Models;
using OnlineShop.Models;

namespace OnlineShop.Mapping
{
    public class OrderMapping : Profile
    {
        public OrderMapping() 
        {
            // N^2 time complexity !!!

            CreateMap<Order, OrderResponseDTO>()
                .ForMember(dest => dest.ProductsPicture, opt => opt.MapFrom(src =>
                    src.Products
                        .Select(productOrder => productOrder.Product.Pictures.First())
                        .Select(picture => Convert.ToBase64String(picture.Image))));
        }
    }
}

using AutoMapper;
using OnlineShop.Mapping.Models;
using OnlineShop.Models;

namespace OnlineShop.Mapping
{
    public class OrderMapping : Profile
    {
        public OrderMapping() 
        {
            CreateMap<Order, OrderResponseDTO>();
        }
    }
}

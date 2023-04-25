using AutoMapper;
using OnlineShop.Models;
using OnlineShop.Shared.DTO.ProductDTO;

namespace OnlineShop.Mapping
{
    public class ReviewMapping : Profile
    {
        public ReviewMapping()
        {
            CreateMap<Review, ReviewDTO>();
        }
    }
}

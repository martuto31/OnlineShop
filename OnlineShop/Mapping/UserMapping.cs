using AutoMapper;
using OnlineShop.Models;
using OnlineShop.Shared.DTO.UserDTO;

namespace OnlineShop.Mapping
{
    public class UserMapping : Profile
    {
        public UserMapping()
        {
            CreateMap<User, UserDTO>();
        }
    }
}

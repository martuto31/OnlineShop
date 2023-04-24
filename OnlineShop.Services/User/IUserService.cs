using OnlineShop.Shared.DTO.UserDTO;

namespace OnlineShop.Services.User
{
    public interface IUserService
    {
        Task AddUserAsync(UserDTO input);
        Task DeleteUserAsync(int id);
        Task UpdateUserAsync(UserDTO userDTO);
        Task<Models.User?> GetUserByIdAsync(int id);
        Task<Models.User?> LoginAsync(LoginDTO userInput);
    }
}

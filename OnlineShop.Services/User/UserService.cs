using OnlineShop.DAL.Repository.User;
using OnlineShop.Shared.DTO.UserDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Services.User
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;

        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task AddUserAsync(UserDTO input)
        {
            var user = new Models.User()
            {
                Email = input.Email,
                Password = input.Password,
                Username = input.Username,
                RoleId = input.RoleId,
            };

            await userRepository.AddUserAsync(user);
            await userRepository.SaveChangesAsync();
        }

        public async Task<Models.User?> LoginAsync(LoginDTO userInput)
        {
            var user = await userRepository.GetUserByUsernameAsync(userInput.Username);

            if(user == null)
            {
                return null;
            }

            if(userInput.Password != user.Password)
            {
                return null;
            }

            return user;
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await userRepository.GetUserByIdAsync(id);

            if(user == null)
            {
                throw new Exception("User should not be null");
            }

            userRepository.DeleteUser(user);
            await userRepository.SaveChangesAsync();
        }

        public async Task<Models.User?> GetUserByIdAsync(int id)
        {
            var user = await userRepository.GetUserByIdAsync(id);

            if(user == null)
            {
                throw new Exception("Unknown user with the provided id.");
            }

            return user;
        }

        public async Task UpdateUserAsync(UserDTO userDTO)
        {
            var user = await userRepository.GetUserByIdAsync(userDTO.Id);

            if(user == null)
            {
                throw new Exception("Can't find user with provided details.");
            }

            user.Username = userDTO.Username;
            user.Password = userDTO.Password;
            user.Email = userDTO.Email;
            user.RoleId = userDTO.RoleId;

            userRepository.UpdateUser(user);
            await userRepository.SaveChangesAsync();
        }
    }
}

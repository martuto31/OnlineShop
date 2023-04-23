using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.DAL.Repository.User
{
    public interface IUserRepository : IGenericRepository<Models.User>
    {
        Task AddUserAsync(Models.User user);
        void DeleteUser(Models.User user);
        void UpdateUser(Models.User user);
        Task<Models.User?> GetUserById(int id);
    }
}

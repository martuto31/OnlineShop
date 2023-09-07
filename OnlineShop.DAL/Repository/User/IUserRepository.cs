using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.DAL.Repository.User
{
    public interface IUserRepository
    {
        Task<Models.User?> GetUserByIdAsync(string id);
    }
}

using Microsoft.EntityFrameworkCore;
using OnlineShop.DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.DAL.Repository.User
{
    public class UserRepository : GenericRepository<Models.User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext) { }

        public async Task AddUserAsync(Models.User user)
        {
            await _context.Users.AddAsync(user);
        }

        public void DeleteUser(Models.User user)
        {
            _context.Users.Remove(user);
        }

        public async Task<Models.User?> GetUserByIdAsync(int id)
        {
            return await DbSet
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<Models.User?> GetUserByUsernameAsync(string username)
        {
            return await DbSet
                .Where(x => x.Username == username)
                .Include(x => x.Role)
                .FirstOrDefaultAsync();
        }

        public void UpdateUser(Models.User user)
        {
            _context.Users.Update(user);
        }
    }
}

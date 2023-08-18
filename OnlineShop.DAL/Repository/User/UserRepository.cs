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
        public UserRepository(ApplicationDbContext context ) : base(context) { }

        public async Task<Models.User?> GetUserByIdAsync(string id)
        {
            return await _context
                .Users
                .Where(x => x.Id == id)
                .Include(x => x.Products)
                .ThenInclude(pr => pr.Product)
                .FirstOrDefaultAsync();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using OnlineShop.DAL.Data;
using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.DAL.Repository.Product
{
    public class ProductColorRepository : GenericRepository<ProductColors>, IProductColorRepository
    {
        public ProductColorRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ProductColors>> GetAllProductColorsAsync()
        {
            return await DbSet
                .ToListAsync();
        }
    }
}

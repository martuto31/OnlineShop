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
    public class ProductSizeRepository : GenericRepository<ProductSizes>, IProductSizeRepository
    {
        public ProductSizeRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
        }

        public async Task<IEnumerable<ProductSizes>> GetAllProductSizesAsync()
        {
            return await DbSet
                .ToListAsync();
        }
    }
}

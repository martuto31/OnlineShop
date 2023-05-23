using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineShop.DAL.Data;
using OnlineShop.Models;
using OnlineShop.Models.Enums;

namespace OnlineShop.DAL.Repository.Product
{
    public class ProductRepository : GenericRepository<Models.Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
        }

        public async Task AddProductAsync(Models.Product product)
        {
            await _context.Products.AddAsync(product);
        }

        public void DeleteProduct(Models.Product product)
        {
            _context.Products.Remove(product);
        }

        public async Task<IEnumerable<Models.Product>> GetAllProductsAsync()
        {
            return await DbSet
                .ToListAsync();
        }
        public async Task<IEnumerable<Models.Product>> GetProductsByTypeAsync(string type)
        {
            var productType = Enum.Parse<ProductType>(type);

            return await DbSet
                .Where(x => x.ProductType == productType)
                .ToListAsync();
        }

        public async Task<Models.Product?> GetProductByIdAsync(int id)
        {
            return await DbSet
                .Where(x => x.Id == id)
                .Include(x => x.ProductsWithSizes)
                    .ThenInclude(ps => ps.ProductSizes)
                .Include(x => x.ProductsWithColors)
                    .ThenInclude(ps => ps.ProductColors)
                .FirstOrDefaultAsync();
        }

        public void UpdateProduct(Models.Product product)
        {
            _context.Products.Update(product);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineShop.DAL.Data;
using OnlineShop.Models;
using OnlineShop.Models.Enums;
using OnlineShop.Shared;
using OnlineShop.Shared.DTO.ProductDTO;

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
        public IQueryable<Models.Product> GetProductsByTypeAsync(string type, int skipCount)
        {
            var productType = Enum.Parse<ProductType>(type);

            return DbSet
                .Where(x => x.ProductType == productType)
                .Skip(skipCount)
                .Take(12);
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

        public async Task<IEnumerable<Models.Product?>> GetFilteredProductsAsync(ProductFilterDTO productFilterDTO, int skipCount)
        {
            var filteredPlants = DbSet.Where(x => x.ProductType == productFilterDTO.productType).AsQueryable();

            if (productFilterDTO.LightIntensities != null && productFilterDTO.LightIntensities.Any())
            {
                filteredPlants = filteredPlants.Where(p => productFilterDTO.LightIntensities.Contains(p.LightIntensity));
            }

            //if (productFilterDTO.Sizes != null && productFilterDTO.Sizes.Any())
            //{
            //    filteredPlants = filteredPlants.Where(p => productFilterDTO.Sizes.Contains());
            //}

            if (productFilterDTO.PetCompatibility || productFilterDTO.AirPurifiable)
            {
                filteredPlants = filteredPlants.Where(p =>
                    (productFilterDTO.PetCompatibility && p.PetCompatibility) ||
                    (productFilterDTO.AirPurifiable && p.AirPurify));
            }

            //if (productFilterDTO.Colors != null && productFilterDTO.Colors.Any())
            //{
            //    filteredPlants = filteredPlants.Where(p => p.Colors.Any(c => productFilterDTO.Colors.Contains(c)));
            //}

            if (productFilterDTO.GrowDifficulties != null && productFilterDTO.GrowDifficulties.Any())
            {
                filteredPlants = filteredPlants.Where(p => productFilterDTO.GrowDifficulties.Contains(p.GrowDifficulty));
            }

            return await filteredPlants
                .Skip(skipCount)
                .Take(12)
                .ToListAsync();
        }

        public void UpdateProduct(Models.Product product)
        {
            _context.Products.Update(product);
        }
    }
}

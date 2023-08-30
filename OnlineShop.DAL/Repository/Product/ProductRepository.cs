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
        public IQueryable<Models.Product> GetProductsByType(string type, int skipCount)
        {
            var productType = Enum.Parse<ProductType>(type);

            return DbSet
                .Where(x => x.ProductType == productType)
                .Skip(skipCount)
                .Take(24);
        }

        public async Task<Models.Product?> GetProductByIdAsync(int id)
        {
            return await DbSet
                .Where(x => x.Id == id)
                .Include(x => x.ProductsWithSizes)
                    .ThenInclude(ps => ps.ProductSizes)
                .Include(x => x.ProductsWithColors)
                    .ThenInclude(ps => ps.ProductColors)
                .Include(x => x.Users)
                    .ThenInclude(ps => ps.User)
                .FirstOrDefaultAsync();
        }

        public IQueryable<Models.Product> GetNewestProducts(string type, int skipCount)
        {
            var productType = Enum.Parse<ProductType>(type);

            var products = DbSet
                .Where(x => x.ProductType == productType)
                .OrderByDescending(x => x.CreatedOn)
                .Skip(skipCount)
                .Take(24);

            return products;
        }

        public IQueryable<Models.Product> GetMostSoldProducts(string type, int skipCount)
        {
            var productType = Enum.Parse<ProductType>(type);

            return DbSet
                .AsQueryable()
                .Where(x => x.ProductType == productType)
                .OrderByDescending(x => x.Sales)
                .Skip(skipCount)
                .Take(24);
        }

        public IQueryable<Models.Product> GetAllUserFavouriteProducts(string userId)
        {
            return _context.UserWithProducts
                .Where(x => x.UserId == userId)
                .Select(x => x.Product)
                .AsQueryable();
        }

        public async Task<IEnumerable<Models.Product>> GetFilteredAndSortedProductsAsync(ProductFilterDTO productFilterDTO, int skipCount, SortType sort)
        {
            IQueryable<Models.Product> filteredPlants;
            string sortType = sort.ToString();

            switch(sortType)
            {
                case "OrderByPriceDesc":
                    filteredPlants = DbSet
                        .AsQueryable()
                        .Where(x => x.ProductType == productFilterDTO.productType)
                        .OrderByDescending(x => x.Price);
                    break;

                case "OrderByPriceAsc":
                    filteredPlants = DbSet
                        .AsQueryable()
                        .Where(x => x.ProductType == productFilterDTO.productType)
                        .OrderBy(x => x.Price);
                    break;

                case "GetNewest":
                    filteredPlants = DbSet
                        .AsQueryable()
                        .Where(x => x.ProductType == productFilterDTO.productType)
                        .OrderByDescending(x => x.CreatedOn);
                    break;

                case "GetMostSold":
                    filteredPlants = DbSet
                        .AsQueryable()
                        .Where(x => x.ProductType == productFilterDTO.productType)
                        .OrderByDescending(x => x.Sales);
                    break;

                default:
                    filteredPlants = DbSet
                        .AsQueryable()
                        .Where(x => x.ProductType == productFilterDTO.productType);
                    break;
            }

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

            int totalProductCount = filteredPlants.Count();
            int nextSkipCount = skipCount + 24;
            bool hasMoreProducts = nextSkipCount < totalProductCount;

            return await filteredPlants
                .Skip(skipCount)
                .Take(24)
                .ToListAsync();
        }

        public bool HasMoreProducts(ProductFilterDTO productFilterDTO, int skipCount, string sortType)
        {
            IQueryable<Models.Product> filteredPlants;

            switch (sortType)
            {
                case "OrderByPriceDesc":
                    filteredPlants = DbSet
                        .AsQueryable()
                        .Where(x => x.ProductType == productFilterDTO.productType)
                        .OrderByDescending(x => x.Price);
                    break;

                case "OrderByPriceAsc":
                    filteredPlants = DbSet
                        .AsQueryable()
                        .Where(x => x.ProductType == productFilterDTO.productType)
                        .OrderBy(x => x.Price);
                    break;

                case "GetNewest":
                    filteredPlants = DbSet
                        .AsQueryable()
                        .Where(x => x.ProductType == productFilterDTO.productType)
                        .OrderByDescending(x => x.CreatedOn);
                    break;

                case "GetMostSold":
                    filteredPlants = DbSet
                        .AsQueryable()
                        .Where(x => x.ProductType == productFilterDTO.productType)
                        .OrderByDescending(x => x.Sales);
                    break;

                default:
                    filteredPlants = DbSet
                        .AsQueryable()
                        .Where(x => x.ProductType == productFilterDTO.productType);
                    break;
            }

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

            int totalProductCount = filteredPlants.Count();
            int nextSkipCount = skipCount + 24;
            bool hasMoreProducts = nextSkipCount < totalProductCount;

            return hasMoreProducts;
        }

        public void UpdateProduct(Models.Product product)
        {
            _context.Products.Update(product);
        }
    }
}

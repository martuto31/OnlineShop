﻿using OnlineShop.Models.Enums;
using OnlineShop.Shared.DTO.ProductDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.DAL.Repository.Product
{
    public interface IProductRepository : IGenericRepository<Models.Product>
    {
        Task<IEnumerable<Models.Product>> GetAllProductsAsync();
        Task<Models.Product?> GetProductByIdAsync(int id);
        IQueryable<Models.Product> GetProductsByType(string type, int skipCount);
        IQueryable<Models.Product> GetNewestProducts(string type, int skipCount);
        IQueryable<Models.Product> GetMostSoldProducts(string type, int skipCount);
        IQueryable<Models.Product> GetAllUserFavouriteProducts(string userId);
        Task<IEnumerable<Models.Product>> GetFilteredAndSortedProductsAsync(ProductFilterDTO productFilterDTO, int skipCount, SortType sortType);
        Task AddProductAsync(Models.Product product);
        bool HasMoreProducts(ProductFilterDTO productFilterDTO, int skipCount, string sortType);
        void UpdateProduct(Models.Product product);
        void DeleteProduct (Models.Product product);
    }
}

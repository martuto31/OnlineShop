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
        IQueryable<Models.Product> GetProductsByTypeAsync(string type, int skipCount);
        IQueryable<Models.Product> GetNewestProducts(string type, int skipCount);
        IQueryable<Models.Product> GetMostSoldProducts(string type, int skipCount);
        Task<IEnumerable<Models.Product?>> GetFilteredAndSortedProductsAsync(ProductFilterDTO productFilterDTO, int skipCount, string sortType);
        Task AddProductAsync(Models.Product product);
        bool HasMoreProducts(ProductFilterDTO productFilterDTO, int skipCount, string sortType);
        void UpdateProduct(Models.Product product);
        void DeleteProduct (Models.Product product);
    }
}

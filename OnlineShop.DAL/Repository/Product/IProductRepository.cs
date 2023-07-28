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
        Task<IEnumerable<Models.Product?>> GetFilteredProductsAsync(ProductFilterDTO productFilterDTO, int skipCount);
        Task AddProductAsync(Models.Product product);
        void UpdateProduct(Models.Product product);
        void DeleteProduct (Models.Product product);
    }
}

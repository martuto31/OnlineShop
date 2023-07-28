using Microsoft.AspNetCore.Http;
using OnlineShop.Models;
using OnlineShop.Shared.DTO.ProductDTO;

namespace OnlineShop.Services.Product
{
    public interface IProductService
    {
        Task<IEnumerable<Models.Product>> GetAllProductsAsync();
        Task<Models.Product> GetProductByIdAsync(int id);
        Task<IEnumerable<Models.Product>> GetProductsByTypeAsync(string type, int skipCount);
        Task<IEnumerable<Models.Product>> GetFilteredProductsAsync(ProductFilterDTO filter, int skipCount);
        Task<IEnumerable<Models.Product>> GetProductsSortedByPriceAscAsync(string type, int skipCount);
        Task<IEnumerable<Models.Product>> GetProductsSortedByPriceDescAsync(string type, int skipCount);
        Task<IEnumerable<ProductColors>> GetAllProductColorsAsync();
        Task AddProductAsync(CreateProductDTO input);
        Task EditProductAsync(CreateProductDTO input);
        Task DeleteProductAsync(int id);
    }
}

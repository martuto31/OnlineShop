using Microsoft.AspNetCore.Http;
using OnlineShop.Models;
using OnlineShop.Shared.DTO.ProductDTO;

namespace OnlineShop.Services.Product
{
    public interface IProductService
    {
        Task<IEnumerable<Models.Product>> GetAllProductsAsync();
        Task<Models.Product> GetProductByIdAsync(int id);
        Task<IEnumerable<Models.Product>> GetProductsByTypeAsync(string type);
        Task<IEnumerable<ProductColors>> GetAllProductColorsAsync();
        Task AddProductAsync(CreateProductDTO input);
        Task EditProductAsync(CreateProductDTO input);
        Task DeleteProductAsync(int id);
    }
}

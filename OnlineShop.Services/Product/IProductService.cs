using OnlineShop.Shared.DTO.ProductDTO;

namespace OnlineShop.Services.Product
{
    public interface IProductService
    {
        Task<IEnumerable<Models.Product>> GetAllProductsAsync();
        Task<Models.Product> GetProductByIdAsync(int id);
        Task<IEnumerable<Models.Product>> GetProductsByTypeAsync(string type);
        Task AddProductAsync(ProductDTO input);
        Task EditProductAsync(ProductDTO input);
        Task DeleteProductAsync(int id);
    }
}

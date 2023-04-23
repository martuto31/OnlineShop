using OnlineShop.Shared.DTO.ProductDTO;

namespace OnlineShop.Services.Product
{
    public interface IProductService
    {
        Task<IEnumerable<Models.Product>> GetAllProductsAsync();
        Task<Models.Product> GetProductByIdAsync(int id);
        Task AddProductAsync(ProductDTO input);
        Task EditProductAsync(ProductDTO input);
        Task DeleteProductAsync(int id);
    }
}

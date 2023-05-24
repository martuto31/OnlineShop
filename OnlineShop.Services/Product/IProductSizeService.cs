using OnlineShop.Models;

namespace OnlineShop.Services.Product
{
    public interface IProductSizeService
    {
        Task<IEnumerable<ProductSizes>> GetAllProductSizesAsync();
    }
}

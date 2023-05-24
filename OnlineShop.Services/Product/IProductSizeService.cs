using OnlineShop.Shared.DTO.ProductDTO;

namespace OnlineShop.Services.Product
{
    public interface IProductSizeService
    {
        Task<IEnumerable<ProductSizesDTO>> GetAllProductSizesAsync();
    }
}

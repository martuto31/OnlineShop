using Microsoft.AspNetCore.Http;
using OnlineShop.Models;
using OnlineShop.Models.Enums;
using OnlineShop.Shared.DTO.ProductDTO;

namespace OnlineShop.Services.Product
{
    public interface IProductService
    {
        Task<IEnumerable<Models.Product>> GetAllProductsAsync();
        Task<Models.Product> GetProductByIdAsync(int id);
        Task<IEnumerable<Models.Product>> GetProductsByTypeAsync(string type, int skipCount);
        Task<IEnumerable<Models.Product>> GetMostSoldProductsAsync(string type, int skipCount);
        Task<IEnumerable<Models.Product>> GetNewestProductsAsync(string type, int skipCount);
        Task<IEnumerable<Models.Product>> GetFilteredAndSortedProductsAsync(ProductFilterDTO filter, int skipCount, SortType sortType);
        Task<IEnumerable<ProductColors>> GetAllProductColorsAsync();
        Task<IEnumerable<Models.Product>> GetAllUserFavouriteProductsAsync(string userId);
        bool HasMoreProducts(ProductFilterDTO filter, int skipCount, string sortType);
        Task AddProductToUserFavouritesAsync(string userId, int productId);
        Task AddProductAsync(CreateProductDTO input);
        Task EditProductAsync(CreateProductDTO input);
        Task DeleteProductAsync(int id);
        Task DeleteProductFromFavouriteAsync(string userId, int productId);
    }
}

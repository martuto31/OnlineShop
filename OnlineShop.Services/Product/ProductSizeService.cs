using OnlineShop.DAL.Repository.Product;
using OnlineShop.Models;
using OnlineShop.Shared.DTO.ProductDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Services.Product
{
    public class ProductSizeService : IProductSizeService
    {
        private readonly IProductSizeRepository _productSizeRepository;

        public ProductSizeService(IProductSizeRepository productSizeRepository)
        {
            _productSizeRepository = productSizeRepository;
        }

        public async Task<IEnumerable<ProductSizes>> GetAllProductSizesAsync()
        {
            return await _productSizeRepository
                .GetAllProductSizesAsync();
        }
    }
}

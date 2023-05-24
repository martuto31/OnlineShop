using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.DAL.Repository.Product
{
    public interface IProductSizeRepository : IGenericRepository<ProductSizes>
    {
        Task<IEnumerable<ProductSizes>> GetAllProductSizesAsync();
    }
}

using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.DAL.Repository.Product
{
    public interface IProductColorRepository : IGenericRepository<ProductColors>
    {
        Task<IEnumerable<ProductColors>> GetAllProductColorsAsync();
    }
}

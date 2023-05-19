using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Services.Product
{
    public interface IImageService
    {
        Task<IEnumerable<ImageUri>> GetAllImagesForProductAsync(int productId);
    }
}

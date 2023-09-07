using Microsoft.AspNetCore.Http;
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
        Task<IEnumerable<string>> GetAllImagesForProductAsBase64Async(int productId);
        List<ImageUri> GetImageFiles(IList<IFormFile> images);
    }
}

using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.DAL.Repository.Product
{
    public interface IImageRepository : IGenericRepository<ImageUri>
    {
        Task AddImageAsync(ImageUri imageUri);
        void DeleteAllImagesByProductId(int id);
        IQueryable<ImageUri> GetAllImagesForProductAsync(int productId);
        void UpdateImage(ImageUri imageUri);
        void DeleteImage(ImageUri imageUri);
    }
}

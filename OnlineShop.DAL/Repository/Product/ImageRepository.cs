using OnlineShop.DAL.Data;
using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.DAL.Repository.Product
{
    public class ImageRepository : GenericRepository<ImageUri>, IImageRepository
    {
        public ImageRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task AddImageAsync(ImageUri imageUri)
        {
            await _context.Images.AddAsync(imageUri);
        }
        public IQueryable<ImageUri> GetAllImagesForProductAsync(int productId)
        {
            return DbSet
                .Where(x => x.ProductId == productId)
                .AsQueryable();
        }

        public void DeleteImage(ImageUri imageUri)
        {
            _context.Images.Remove(imageUri);
        }

        public void UpdateImage(ImageUri imageUri)
        {
            _context.Images.Update(imageUri);
        }
    }
}

using OnlineShop.DAL.Repository.Product;
using OnlineShop.Shared.DTO.ProductDTO;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using static System.Net.Mime.MediaTypeNames;
using System.IO.Pipes;
using OnlineShop.Models;

namespace OnlineShop.Services.Product
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository productRepository;

        public ProductService(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public async Task AddProductAsync(CreateProductDTO input)
        {
            List<ImageUri> images = new List<ImageUri>();

            if (input.Images != null && input.Images.Count > 0)
            {
                foreach (var imageFile in input.Images)
                {
                    byte[] img = ConvertIFormFileToByteArray(imageFile);
                    img = ImageService.CompressAndResizeImage(img, 400, 400);
                    var image = new ImageUri()
                    {
                        Image = img,
                    };
                    images.Add(image);
                }
            }

            var product = new Models.Product()
            {
                Description = input.Description,
                Name = input.Name,
                Price = input.Price,
                ProductTarget = input.ProductTarget,
                ProductType = input.ProductType,
                ProductSizes = input.ProductSizes,
                ProductColors = input.ProductColors,
                Pictures = images,
            };

            await productRepository.AddProductAsync(product);
            await productRepository.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await productRepository.GetProductByIdAsync(id);

            if(product == null)
            {
                throw new Exception("Object should not be null.");
            }

            productRepository.DeleteProduct(product);
            await productRepository.SaveChangesAsync();
        }

        public async Task EditProductAsync(CreateProductDTO input)
        {
            var product = await productRepository.GetProductByIdAsync(input.Id);

            if(product == null)
            {
                throw new Exception("Object should not be null.");
            }

            product.Name = input.Name;
            product.Price = input.Price;
            product.Description = input.Description;
            product.ProductTarget = input.ProductTarget;
            product.ProductType = input.ProductType;
            product.ProductColors = input.ProductColors;
            product.ProductSizes = input.ProductSizes;
            //product.Picture = ConvertIFormFileToByteArray(input.Image);

            //product.Picture = ImageService.CompressAndResizeImage(product.Picture, 400, 400);

            productRepository.UpdateProduct(product);
            await productRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<Models.Product>> GetAllProductsAsync()
        {
            var products = await productRepository.GetAllProductsAsync();

            return products;
        }

        public async Task<Models.Product> GetProductByIdAsync(int id)
        {
            var product = await productRepository.GetProductByIdAsync(id);

            if(product == null)
            {
                throw new Exception("Object should not be null.");
            }

            return product;
        }

        public async Task<IEnumerable<Models.Product>> GetProductsByTypeAsync(string type)
        {
            var products = await productRepository.GetProductsByTypeAsync(type);

            if(products == null)
            {
                throw new Exception("Object should not be null.");
            }

            return products;
        }

        private byte[] ConvertIFormFileToByteArray(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                file.CopyToAsync(memoryStream).GetAwaiter().GetResult();
                return memoryStream.ToArray();
            }
        }
    }
}

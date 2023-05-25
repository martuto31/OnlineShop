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
        private readonly IProductColorRepository productColorRepository;

        public ProductService(IProductRepository productRepository, IProductColorRepository productColorRepository)
        {
            this.productRepository = productRepository;
            this.productColorRepository = productColorRepository;
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

            var productsWithColors = new List<ProductsWithColors>();

            if (input.ProductColors != null)
            {
                foreach (var color in input.ProductColors)
                {
                    var productWithColors = new ProductsWithColors()
                    {
                        ProductColorsId = color,
                        ProductId = input.Id,
                    };

                    productsWithColors.Add(productWithColors);
                }
            }

            var productsWithSizes = new List<ProductsWithSizes>();

            if (input.ProductSizes != null)
            {
                foreach (var size in input.ProductSizes)
                {
                    var productWithSizes = new ProductsWithSizes()
                    {
                        ProductSizesId = size,
                        ProductId = input.Id,
                    };

                    productsWithSizes.Add(productWithSizes);
                }
            }

            var product = new Models.Product()
            {
                Description = input.Description,
                Name = input.Name,
                Price = input.Price,
                ProductTarget = input.ProductTarget,
                ProductType = input.ProductType,
                ProductsWithColors = productsWithColors,
                ProductsWithSizes = productsWithSizes,
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
            //product.ProductColors = input.ProductColors;
            //product.ProductSizes = input.ProductSizes;
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

        public async Task<IEnumerable<ProductColors>> GetAllProductColorsAsync()
        {
            var colors = await productColorRepository.GetAllProductColorsAsync();

            if(colors == null)
            {
                throw new Exception("No colors found in database.");
            }

            return colors;
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

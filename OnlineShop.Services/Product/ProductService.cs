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
using OnlineShop.Models.Enums;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace OnlineShop.Services.Product
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository productRepository;
        private readonly IProductColorRepository productColorRepository;
        private readonly IImageRepository imageRepository;

        public ProductService(IProductRepository productRepository, IProductColorRepository productColorRepository, IImageRepository imageRepository)
        {
            this.productRepository = productRepository;
            this.productColorRepository = productColorRepository;
            this.imageRepository = imageRepository;
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
                ProductsWithColors = productsWithColors,
                ProductsWithSizes = productsWithSizes,
                Pictures = images,
                PetCompatibility = input.PetCompatibility,
                AirPurify = input.AirPurify,
                LightIntensity = input.LightIntensity,
                GrowDifficulty = input.GrowDifficulty,
                ProductType = input.ProductType,
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

            if (product == null)
            {
                throw new Exception("Object should not be null.");
            }

            List<ImageUri> images = new List<ImageUri>();

            if (input.Images != null)
            {
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

                this.imageRepository.DeleteAllImagesByProductId(product.Id);

                product.Pictures = images;
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

            product.Name = input.Name;
            product.Price = input.Price;
            product.Description = input.Description;
            product.ProductsWithColors = productsWithColors;
            product.ProductsWithSizes = productsWithSizes;
            product.AirPurify = input.AirPurify;
            product.PetCompatibility = input.PetCompatibility;
            product.LightIntensity = input.LightIntensity;
            product.GrowDifficulty = input.GrowDifficulty;
            product.ProductType = input.ProductType;

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

        public async Task<IEnumerable<Models.Product>> GetProductsByTypeAsync(string type, int skipCount)
        {
            var products =  await productRepository.GetProductsByTypeAsync(type, skipCount).ToListAsync();

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

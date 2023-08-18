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
using Microsoft.AspNetCore.Identity;
using OnlineShop.DAL.Repository.User;

namespace OnlineShop.Services.Product
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository productRepository;
        private readonly IProductColorRepository productColorRepository;
        private readonly IImageRepository imageRepository;
        private readonly IUserRepository userRepository;
        private readonly UserManager<Models.User> userManager;

        public ProductService(IProductRepository productRepository, IProductColorRepository productColorRepository, 
                                IImageRepository imageRepository, IUserRepository userRepository, UserManager<Models.User> userManager)
        {
            this.productRepository = productRepository;
            this.productColorRepository = productColorRepository;
            this.imageRepository = imageRepository;
            this.userManager = userManager;
            this.userRepository = userRepository;
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
                AdditionalDescription = input.AdditionalDescription,
                Name = input.Name,
                BotanicalName = input.BotanicalName,
                CommonName = input.CommonName,
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

            if (product == null)
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
            product.CommonName = input.CommonName;
            product.BotanicalName = input.BotanicalName;
            product.Price = input.Price;
            product.AdditionalDescription = input.AdditionalDescription;
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

            if (product == null)
            {
                throw new Exception("Object should not be null.");
            }

            return product;
        }

        public async Task<IEnumerable<Models.Product>> GetProductsByTypeAsync(string type, int skipCount)
        {
            var products = await productRepository.GetProductsByTypeAsync(type, skipCount).ToListAsync();

            if (products == null)
            {
                throw new Exception("Object should not be null.");
            }

            return products;
        }

        public async Task<IEnumerable<Models.Product>> GetNewestProductsAsync(string type, int skipCount)
        {
            var products = await productRepository.GetNewestProducts(type, skipCount).ToListAsync();

            if (products == null)
            {
                throw new Exception("Object should not be null.");
            }

            return products;
        }

        public async Task<IEnumerable<Models.Product>> GetMostSoldProductsAsync(string type, int skipCount)
        {
            var products = await productRepository.GetMostSoldProducts(type, skipCount).ToListAsync();

            if (products == null)
            {
                throw new Exception("Object should not be null.");
            }

            return products;
        }

        public async Task<IEnumerable<Models.Product>> GetFilteredAndSortedProductsAsync(ProductFilterDTO filter, int skipCount, string sortType)
        {
            var products = await productRepository.GetFilteredAndSortedProductsAsync(filter, skipCount, sortType);

            if (products == null)
            {
                throw new Exception("Object should not be null.");
            }

            return products;
        }

        public async Task<IEnumerable<ProductColors>> GetAllProductColorsAsync()
        {
            var colors = await productColorRepository.GetAllProductColorsAsync();

            if (colors == null)
            {
                throw new Exception("No colors found in database.");
            }

            return colors;
        }

        public async Task<IEnumerable<Models.Product>> GetAllUserFavouriteProducts(string userId)
        {
            var products = await productRepository.GetAllUserFavouriteProducts(userId).ToListAsync();

            if (!products.Any())
            {
                throw new Exception("Потребителят няма продукти под категория 'любими'.");
            }

            return products;
        }

        public async Task AddProductToUserFavouritesAsync(string userId, int productId)
        {
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new Exception("Не съществува такъв акаунт.");
            }

            var product = await productRepository.GetProductByIdAsync(productId);

            if (product == null)
            {
                throw new Exception("Не съществува такъв продукт.");
            }

            product.Users.Add(new UserWithProducts { UserId = user.Id, ProductId = product.Id });
            await productRepository.SaveChangesAsync();
        }

        public async Task DeleteProductFromFavourite(string userId, int productId)
        {
            var user = await userRepository.GetUserByIdAsync(userId);

            if (user == null)
            {
                throw new Exception("Не съществува такъв акаунт.");
            }

            var product = user.Products.FirstOrDefault(x => x.ProductId == productId);

            if (product == null)
            {
                throw new Exception("Не съществува такъв продукт.");
            }

            user.Products.Remove(product);
            await productRepository.SaveChangesAsync();
        }

        public bool HasMoreProducts(ProductFilterDTO filter, int skipCount, string sortType)
        {
            return productRepository.HasMoreProducts(filter, skipCount, sortType);
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

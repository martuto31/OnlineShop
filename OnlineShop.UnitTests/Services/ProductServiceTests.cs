using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using OnlineShop.DAL.Data;
using OnlineShop.DAL.Repository.Product;
using OnlineShop.DAL.Repository.User;
using OnlineShop.Models;
using OnlineShop.Models.Enums;
using OnlineShop.Services.Product;
using OnlineShop.Shared.DTO.ProductDTO;
using OnlineShop.UnitTests.Helpers;
using SixLabors.ImageSharp.Processing.Processors.Quantization;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Net.Sockets;
using System.Reflection.Metadata;
using Xunit;

namespace OnlineShop.UnitTests.Services
{
    public class ProductServiceTests
    {
        private readonly Mock<IProductRepository> _productRepoMock;
        private readonly Mock<IProductColorRepository> _productColorRepoMock;
        private readonly Mock<IImageRepository> _imageRepoMock;
        private readonly Mock<IUserRepository> _userRepoMock;
        private readonly Mock<UserManager<User>> _userManagerMock;
        private readonly Mock<DbSet<Product>> _productsSetMock;
        private readonly Mock<ApplicationDbContext> _applicationDbContextMock;
        private readonly Mock<IImageService> _imageServiceMock;
        private IProductRepository _productRepository;
        private IProductService _productService;
        private IFixture _fixture;

        public ProductServiceTests()
        {
            SetupFixture();

            _productRepoMock = _fixture.Freeze<Mock<IProductRepository>>();
            _productColorRepoMock = _fixture.Freeze<Mock<IProductColorRepository>>();
            _imageRepoMock = _fixture.Freeze<Mock<IImageRepository>>();
            _userRepoMock = _fixture.Freeze<Mock<IUserRepository>>();
            _userManagerMock = _fixture.Freeze<Mock<UserManager<User>>>();
            _productsSetMock = new Mock<DbSet<Product>>();
            _applicationDbContextMock = new Mock<ApplicationDbContext>();
            _imageServiceMock = new Mock<IImageService>();

            _productService = new ProductService(
                _productRepoMock.Object,
                _productColorRepoMock.Object,
                _imageRepoMock.Object,
                _userRepoMock.Object,
                _userManagerMock.Object,
                _imageServiceMock.Object
            );
        }

        [Theory]
        [CustomAutoData]
        public async Task DeleteProductAsync_ProductExists_DeletesProduct([Frozen] Product product)
        {
            // Arrange
            _productRepoMock.Setup(x => x.GetProductByIdAsync(product.Id))
                .ReturnsAsync(product);

            // Act
            await _productService.DeleteProductAsync(product.Id);

            // Assert
            _productRepoMock.Verify(repo => repo.DeleteProduct(product), Times.Once);
            _productRepoMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Theory]
        [AutoData]
        public async Task DeleteProductAsync_ProductNotFound_ThrowsException(int productId)
        {
            // Arrange
            _productRepoMock.Setup(x => x.GetProductByIdAsync(productId))
                .ReturnsAsync((Product)null); // Simulate product not found

            // Act
            await Assert.ThrowsAsync<Exception>(async () => await _productService.DeleteProductAsync(productId));

            // Assert
            _productRepoMock.Verify(repo => repo.DeleteProduct(It.IsAny<Product>()), Times.Never);
            _productRepoMock.Verify(repo => repo.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task GetAllProductsAsync_ProductsExist_ReturnsProducts()
        {
            // Arrange
            IEnumerable<Product> products = _fixture.Build<Product>().CreateMany(5);
            _productRepoMock.Setup(x => x.GetAllProductsAsync())
                .ReturnsAsync(products);

            // Act
            var result = await _productService.GetAllProductsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(products, result);
            Assert.Equal(products.Count(), result.Count());
            Assert.IsAssignableFrom<IEnumerable<Product>>(products);
        }

        [Fact]
        public async Task GetAllProductsAsync_NoProducts_EmptyCollection()
        {
            // Arrange
            IEnumerable<Product> products = Enumerable.Empty<Product>();
            _productRepoMock.Setup(x => x.GetAllProductsAsync())
                .ReturnsAsync(products);

            // Act
            var result = await _productService.GetAllProductsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Theory, CustomAutoData]
        public async Task GetProductByIdAsync_ProductExists_ReturnsProduct([Frozen] Product product)
        {
            // Arrange
            _productRepoMock.Setup(x => x.GetProductByIdAsync(product.Id))
                .ReturnsAsync(product);

            // Act
            var result = await _productService.GetProductByIdAsync(product.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(product.Id, result.Id);
        }

        [Theory, CustomAutoData]
        public async Task GetProductByIdAsync_ProductNotFound_ThrowsException([Frozen] Product product)
        {
            // Arrange
            _productRepoMock.Setup(x => x.GetProductByIdAsync(product.Id))
                .ReturnsAsync((Product)null);

            // Act
            var exception = await Assert.ThrowsAsync<Exception>(() => _productService.GetProductByIdAsync(product.Id));

            // Assert
            // HARDCODED STRING!!!
            Assert.Equal("Object should not be null.", exception.Message);
        }

        [Theory, CustomAutoData]
        public async Task GetProductsByTypeAsync_ProductsExist_ReturnsProducts(ProductType productType)
        {
            // Arrange
            var products = _fixture.Build<Product>()
                .With(x => x.ProductType, productType)
                .CreateMany(5)
                .AsQueryable();

            var mockDbSet = GetMockDbSet<Product>(products);

            _productRepoMock.Setup(x => x.GetProductsByType(productType.ToString(), It.IsAny<int>()))
                            .Returns(mockDbSet.Object);

            // Act
            var result = await _productService.GetProductsByTypeAsync(productType.ToString(), It.IsAny<int>());

            // Assert
            Assert.NotNull(result);
            Assert.Equal(products, result);
            Assert.Equal(products.Count(), result.Count());
            Assert.IsAssignableFrom<IEnumerable<Product>>(products);
            Assert.All(result, item => Assert.Contains(productType.ToString(), item.ProductType.ToString()));
        }

        [Fact]
        public async Task GetProductsByTypeAsync_NoProducts__ReturnsEmptyCollection()
        {
            // Arrange
            var emptyProducts = Enumerable.Empty<Product>().AsQueryable();
            var mockDbSet = GetMockDbSet<Product>(emptyProducts);

            _productRepoMock.Setup(x => x.GetProductsByType(It.IsAny<string>(), It.IsAny<int>()))
                            .Returns((mockDbSet.Object));

            // Act
            var result = await _productService.GetProductsByTypeAsync(It.IsAny<string>(), It.IsAny<int>());

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Theory, CustomAutoData]
        public async Task GetNewestProductsAsync_ProductsExist_ReturnsNewestProducts(ProductType productType)
        {
            // Arrange
            var products = _fixture.Build<Product>()
                .With(x => x.ProductType, productType)
                .CreateMany(5)
                .OrderByDescending(x => x.CreatedOn)
                .AsQueryable();

            var mockDbSet = GetMockDbSet<Product>(products);

            _productRepoMock.Setup(x => x.GetNewestProducts(productType.ToString(), It.IsAny<int>()))
                            .Returns(mockDbSet.Object);

            // Act
            var result = await _productService.GetNewestProductsAsync(productType.ToString(), It.IsAny<int>());

            // Assert
            Assert.NotNull(result);
            Assert.Equal(products, result);
            Assert.Equal(products.Count(), result.Count());
            Assert.IsAssignableFrom<IEnumerable<Product>>(products);
            Assert.All(result, item => Assert.Contains(productType.ToString(), item.ProductType.ToString()));
        }

        [Fact]
        public async Task GetNewestProductsAsync_NoProducts__ReturnsEmptyCollection()
        {
            // Arrange
            var emptyProducts = Enumerable.Empty<Product>().AsQueryable();
            var mockDbSet = GetMockDbSet<Product>(emptyProducts);

            _productRepoMock.Setup(x => x.GetNewestProducts(It.IsAny<string>(), It.IsAny<int>()))
                            .Returns((mockDbSet.Object));

            // Act
            var result = await _productService.GetNewestProductsAsync(It.IsAny<string>(), It.IsAny<int>());

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Theory, CustomAutoData]
        public async Task GetMostSoldProductsAsync_ProductsExist_ReturnsMostSoldProducts(ProductType productType)
        {
            // Arrange
            var products = _fixture.Build<Product>()
                .With(x => x.ProductType, productType)
                .CreateMany(5)
                .OrderByDescending(x => x.Sales)
                .AsQueryable();

            var mockDbSet = GetMockDbSet<Product>(products);

            _productRepoMock.Setup(x => x.GetMostSoldProducts(productType.ToString(), It.IsAny<int>()))
                            .Returns(mockDbSet.Object);

            // Act
            var result = await _productService.GetMostSoldProductsAsync(productType.ToString(), It.IsAny<int>());

            // Assert
            Assert.NotNull(result);
            Assert.Equal(products, result);
            Assert.Equal(products.Count(), result.Count());
            Assert.IsAssignableFrom<IEnumerable<Product>>(products);
            Assert.All(result, item => Assert.Contains(productType.ToString(), item.ProductType.ToString()));
        }

        [Fact]
        public async void GetMostSoldProductsAsync_NoProducts__ReturnsEmptyCollection()
        {
            // Arrange
            var emptyProducts = Enumerable.Empty<Product>().AsQueryable();
            var mockDbSet = GetMockDbSet<Product>(emptyProducts);

            _productRepoMock.Setup(x => x.GetMostSoldProducts(It.IsAny<string>(), It.IsAny<int>()))
                            .Returns((mockDbSet.Object));

            // Act
            var result = await _productService.GetMostSoldProductsAsync(It.IsAny<string>(), It.IsAny<int>());

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Theory, CustomAutoData]
        public async Task GetFilteredAndSortedProductsAsync_ValidProducts_ReturnsProducts(Task<IEnumerable<Product>> products)
        {
            // Arrange
            _productRepoMock.Setup(x => x.GetFilteredAndSortedProductsAsync(It.IsAny<ProductFilterDTO>(), It.IsAny<int>(), It.IsAny<SortType>()))
                .Returns(products);

            // Act
            var result = await _productService.GetFilteredAndSortedProductsAsync(It.IsAny<ProductFilterDTO>(), It.IsAny<int>(), It.IsAny<SortType>());

            // Assert
            Assert.NotNull(result);
            Assert.Equal(products.Result, result);
            Assert.Equal(products.Result.Count(), result.Count());
        }

        [Fact]
        public async Task GetFilteredAndSortedProductsAsync_NoProducts_ReturnsEmptyCollection()
        {
            // Arrange
            var emptyProducts = Enumerable.Empty<Product>().AsEnumerable();
            _productRepoMock.Setup(x => x.GetFilteredAndSortedProductsAsync(It.IsAny<ProductFilterDTO>(), It.IsAny<int>(), It.IsAny<SortType>()))
                .ReturnsAsync(emptyProducts);

            // Act
            var result = await _productService.GetFilteredAndSortedProductsAsync(It.IsAny<ProductFilterDTO>(), It.IsAny<int>(), It.IsAny<SortType>());

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            Assert.IsAssignableFrom<IEnumerable<Product>>(result);
        }

        [Fact]
        public async Task GetAllUserFavouriteProducts_UserHasProducts_ReturnsFavouriteProducts()
        {
            // Arrange
            var products = _fixture.Build<Product>()
                .CreateMany(5)
                .AsQueryable();

            var mockDbSet = this.GetMockDbSet(products);
            _productRepoMock.Setup(x => x.GetAllUserFavouriteProducts(It.IsAny<string>()))
                .Returns(mockDbSet.Object);

            // Act
            var result = await _productService.GetAllUserFavouriteProductsAsync(It.IsAny<string>());

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(products, result);
            Assert.IsAssignableFrom<IEnumerable<Product>>(result);
        }

        [Fact]
        public void GetAllUserFavouriteProducts_NoProducts_ReturnsEmptyCollection()
        {
            // Arrange
            var products = Enumerable.Empty<Product>().AsQueryable();

            var mockDbSet = this.GetMockDbSet(products);
            _productRepoMock.Setup(x => x.GetAllUserFavouriteProducts(It.IsAny<string>()))
                .Returns(mockDbSet.Object);

            // Act & Assert
            var exception = Assert.ThrowsAsync<Exception>(async () => await _productService.GetAllUserFavouriteProductsAsync(It.IsAny<string>()));
        }

        [Theory, CustomAutoData]
        public async Task GetAllProductColorsAsync_ReturnsProducts(IEnumerable<ProductColors> colors)
        {
            // Arrange
            _productColorRepoMock.Setup(x => x.GetAllProductColorsAsync())
                .ReturnsAsync(colors);

            // Act
            var result = await _productService.GetAllProductColorsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(colors, result);
            Assert.IsAssignableFrom<IEnumerable<ProductColors>>(result);
        }

        [Fact]
        public async Task GetAllProductColorsAsync_ReturnsEmptyCollection()
        {
            // Arrange
            var colors = Enumerable.Empty<ProductColors>();
            _productColorRepoMock.Setup(x => x.GetAllProductColorsAsync())
                .ReturnsAsync(colors);

            // Act
            var result = await _productService.GetAllProductColorsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            Assert.Equal(colors, result);
            Assert.IsAssignableFrom<IEnumerable<ProductColors>>(result);
        }

        // TESTING NOTHING - STATIC SERVICE
        [Theory, CustomAutoData]
        public async Task AddProductToUserFavouritesAsync_UserAndProductFound_AddsProductToFavourites(User user, Product product)
        {
            // Arrange
            _userManagerMock.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(user);
            _productRepoMock.Setup(x => x.GetProductByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(product);

            // Act
            await _productService.AddProductToUserFavouritesAsync(It.IsAny<string>(), It.IsAny<int>());

            // Assert
            _productRepoMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Theory, CustomAutoData]
        public async Task AddProductToUserFavouritesAsync_ProductNotFound_ThrowsException(User user)
        {
            // Arrange
            _userManagerMock.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(user);
            _productRepoMock.Setup(x => x.GetProductByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Product)null);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _productService.AddProductToUserFavouritesAsync(It.IsAny<string>(), It.IsAny<int>()));
        }

        [Theory, CustomAutoData]
        public async Task AddProductToUserFavouritesAsync_UserNotFound_ThrowsException(Product product)
        {
            // Arrange
            _userManagerMock.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((User)null);
            _productRepoMock.Setup(x => x.GetProductByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(product);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _productService.AddProductToUserFavouritesAsync(It.IsAny<string>(), It.IsAny<int>()));
        }

        [Fact]
        public async void DeleteProductFromFavouriteAsync_UserNotFound_ThrowsException()
        {
            // Arrange
            _userRepoMock.Setup(x => x.GetUserByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((User)null);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _productService.DeleteProductFromFavouriteAsync(It.IsAny<string>(), It.IsAny<int>()));
        }

        [Fact]
        public async Task DeleteProductFromFavouriteAsync_ProductNotInFavorites_ThrowsException()
        {
            // Arrange
            _userRepoMock.Setup(repo => repo.GetUserByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new User { Id = It.IsAny<string>(), Products = new List<UserWithProducts>() });

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _productService.DeleteProductFromFavouriteAsync(It.IsAny<string>(), It.IsAny<int>()));
        }

        [Fact]
        public async Task DeleteProductFromFavouriteAsync_UserAndProductFound_DeletesFromFavorites()
        {
            // Arrange
            var user = new User { Id = "userId", Products = new List<UserWithProducts> { new UserWithProducts { ProductId = 123 } } };

            _userRepoMock.Setup(repo => repo.GetUserByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(user);
            _productRepoMock.Setup(repo => repo.SaveChangesAsync())
                .Verifiable();

            // Act
            await _productService.DeleteProductFromFavouriteAsync("userId", 123);

            // Assert
            Assert.Empty(user.Products);
            _productRepoMock.Verify();
        }

        [Fact]
        public void HasMoreProducts_NoMoreProducts_ReturnsFalse()
        {
            // Arrange
            _productRepoMock.Setup(x => x.HasMoreProducts(It.IsAny<ProductFilterDTO>(), It.IsAny<int>(), It.IsAny<string>()))
                .Returns(false);

            // Act
            var result = _productService.HasMoreProducts(It.IsAny<ProductFilterDTO>(), It.IsAny<int>(), It.IsAny<string>());

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void HasMoreProducts_HasMoreProducts_ReturnsTrue()
        {
            // Arrange
            _productRepoMock.Setup(x => x.HasMoreProducts(It.IsAny<ProductFilterDTO>(), It.IsAny<int>(), It.IsAny<string>()))
                .Returns(true);

            // Act
            var result = _productService.HasMoreProducts(It.IsAny<ProductFilterDTO>(), It.IsAny<int>(), It.IsAny<string>());

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task EditProductAsync_ValidInput_UpdatesProduct()
        {
            // Arrange
            var existingProduct = new Product { Id = 123 };
            var input = new CreateProductDTO { Id = 123 };
            var images = _fixture.Build<ImageUri>()
                .CreateMany(5)
                .ToList();

            _productRepoMock.Setup(repo => repo.GetProductByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(existingProduct);
            _productRepoMock.Setup(repo => repo.UpdateProduct(It.IsAny<Product>()))
                .Verifiable();
            _productRepoMock.Setup(repo => repo.SaveChangesAsync())
                .Verifiable();
            _imageServiceMock.Setup(x => x.GetImageFiles(It.IsAny<IList<IFormFile>>()))
                .Returns(images);

            // Act
            await _productService.EditProductAsync(input);

            // Assert
            _productRepoMock.Verify(repo => repo.UpdateProduct(It.IsAny<Product>()), Times.Once);
            _productRepoMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Theory, CustomAutoData]
        public async void EditProductAsync_ProductNotFound_ThrowsException(CreateProductDTO createProductDTO)
        {
            // Arrange
            _productRepoMock.Setup(repo => repo.GetProductByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Product)null);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _productService.EditProductAsync(createProductDTO));
        }

        [Fact]
        public async Task AddProductAsync_ValidInput_AddsProductToRepository()
        {
            // Arrange
            var input = new CreateProductDTO { Id = 123 };
            var existingProduct = new Product { Id = 123 };
            var images = _fixture.Build<ImageUri>()
                .CreateMany(5)
                .ToList();

            _productRepoMock.Setup(x => x.AddProductAsync(It.IsAny<Product>()))
                .Verifiable();
            _productRepoMock.Setup(x => x.SaveChangesAsync())
                .Verifiable();
            _imageServiceMock.Setup(x => x.GetImageFiles(It.IsAny<IList<IFormFile>>()))
                .Returns(images);

            // Act
            await _productService.AddProductAsync(input);

            // Assert
            _productRepoMock.VerifyAll();
        }

        private void SetupFixture()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        private Mock<DbSet<T>> GetMockDbSet<T>(IQueryable<T> items) 
            where T : class
        {
            var mockDbSet = new Mock<DbSet<T>>();
            mockDbSet.As<IAsyncEnumerable<T>>()
                .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                .Returns(new TestAsyncEnumerator<T>(items.GetEnumerator()));

            return mockDbSet;
        }
    }
}

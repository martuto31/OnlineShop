using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using OnlineShop.DAL.Data;
using OnlineShop.DAL.Repository.Product;
using OnlineShop.DAL.Repository.User;
using OnlineShop.Models;
using OnlineShop.Models.Enums;
using OnlineShop.Services.Product;
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
        private readonly Mock<DbSet<Product>> _productsSetMock;
        private readonly Mock<ApplicationDbContext> _applicationDbContextMock;
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
            _productsSetMock = new Mock<DbSet<Product>>();
            _applicationDbContextMock = new Mock<ApplicationDbContext>();

            _productService = new ProductService(
                _productRepoMock.Object,
                _productColorRepoMock.Object,
                _imageRepoMock.Object,
                _userRepoMock.Object
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

        public async Task GetFilteredAndSortedProductsAsync_ValidProducts_ReturnsProducts()
        {

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

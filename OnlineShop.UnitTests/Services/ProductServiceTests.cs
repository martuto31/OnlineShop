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
using OnlineShop.UnitTests.Helpers.Products;
using SixLabors.ImageSharp.Processing.Processors.Quantization;
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

        [Theory, CustomAutoData]
        public async Task GetProductsByTypeAsync_ProductsExist_ReturnsProducts(ProductType productType, int skipCount)
        {
            // Arrange
            var products = new List<Product>
            {
                new Product {Id = 1, ProductType = productType},
                new Product {Id = 2, ProductType = productType},
                new Product {Id = 3, ProductType = productType}
            }.AsQueryable();


            var mockDbSet = new Mock<DbSet<Product>>();
            mockDbSet.As<IAsyncEnumerable<Product>>()
                .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                .Returns(new TestAsyncEnumerator<Product>(products.GetEnumerator()));

            _productRepoMock.Setup(x => x.GetProductsByType(productType.ToString(), skipCount))
                            .Returns(mockDbSet.Object);

            _productService = new ProductService(
                _productRepoMock.Object,
                _productColorRepoMock.Object,
                _imageRepoMock.Object,
                _userRepoMock.Object
            );

            // Act
            var result = await _productService.GetProductsByTypeAsync(productType.ToString(), skipCount);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(products, result);
            foreach (var product in result)
            {
                Assert.Equal(productType, product.ProductType);
            }
        }

        private void SetupFixture()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        private void SetupVirtualDB(IQueryable<Product> productsQuery)
        {
            _productsSetMock.As<IQueryable<Product>>().Setup(m => m.Provider).Returns(productsQuery.Provider);
            _productsSetMock.As<IQueryable<Product>>().Setup(m => m.Expression).Returns(productsQuery.Expression);
            _productsSetMock.As<IQueryable<Product>>().Setup(m => m.ElementType).Returns(productsQuery.ElementType);
            _productsSetMock.As<IQueryable<Product>>().Setup(m => m.GetEnumerator()).Returns(() => productsQuery.GetEnumerator());

            _applicationDbContextMock.Setup(x => x.Products).Returns(_productsSetMock.Object);
            _productRepository = new ProductRepository(_applicationDbContextMock.Object);
        }

        private void SetupService()
        {
            _productService = new ProductService(
                _productRepository,
                _productColorRepoMock.Object,
                _imageRepoMock.Object,
                _userRepoMock.Object
            );
        }
    }
    public class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> _enumerator;

        public TestAsyncEnumerator(IEnumerator<T> enumerator)
        {
            _enumerator = enumerator;
        }

        public T Current => _enumerator.Current;

        public ValueTask DisposeAsync()
        {
            _enumerator.Dispose();
            return new ValueTask(Task.CompletedTask);
        }

        public ValueTask<bool> MoveNextAsync()
        {
            return new ValueTask<bool>(_enumerator.MoveNext());
        }
    }
}

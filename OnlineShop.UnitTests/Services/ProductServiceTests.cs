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

        [Theory, CustomAutoData]
        public async Task GetProductsByTypeAsync_ProductsExist_ReturnsProducts(ProductType productType)
        {
            // Arrange
            var products = _fixture.Build<Product>()
                .With(x => x.ProductType, productType)
                .CreateMany(5)
                .AsQueryable();

            var mockDbSet = GetMockDbSet(products);

            _productRepoMock.Setup(x => x.GetProductsByType(productType.ToString(), It.IsAny<int>()))
                            .Returns(mockDbSet.Object);

            // Act
            var result = await _productService.GetProductsByTypeAsync(productType.ToString(), It.IsAny<int>());

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

using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using Microsoft.AspNetCore.Identity;
using Moq;
using OnlineShop.DAL.Repository.Product;
using OnlineShop.DAL.Repository.User;
using OnlineShop.Models;
using OnlineShop.Services.Product;
using OnlineShop.UnitTests.Helpers;
using Xunit;

namespace OnlineShop.UnitTests.Services
{
    public class ProductServiceTests
    {
        private readonly Mock<IProductRepository> _productRepoMock;
        private readonly Mock<IProductColorRepository> _productColorRepoMock;
        private readonly Mock<IImageRepository> _imageRepoMock;
        private readonly Mock<IUserRepository> _userRepoMock;
        private readonly IProductService _productService;
        private IFixture _fixture;

        public ProductServiceTests()
        {
            SetupFixture();
            _productRepoMock = _fixture.Freeze<Mock<IProductRepository>>();
            _productColorRepoMock = _fixture.Freeze<Mock<IProductColorRepository>>();
            _imageRepoMock = _fixture.Freeze<Mock<IImageRepository>>();
            _userRepoMock = _fixture.Freeze<Mock<IUserRepository>>();

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

        private void SetupFixture()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }
    }
}

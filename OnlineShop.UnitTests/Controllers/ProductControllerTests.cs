using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using OnlineShop.Controllers;
using OnlineShop.DAL.Repository.Product;
using OnlineShop.Mapping.Models;
using OnlineShop.Models;
using OnlineShop.Services.Product;
using OnlineShop.Shared.DTO.ProductDTO;
using OnlineShop.UnitTests.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.UnitTests.Controllers
{
    public class ProductControllerTests
    {
        private readonly Mock<SignInManager<User>> _signInManagerMock;
        private readonly Mock<UserManager<User>> _userManagerMock;
        private readonly Mock<IProductService> _productServiceMock;
        private readonly Mock<IProductSizeService> _productSizeServiceMock;
        private readonly Mock<IImageService> _imageServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly ProductController _productController;
        private IFixture _fixture;

        public ProductControllerTests()
        {
            SetupFixture();

            _signInManagerMock = _fixture.Freeze<Mock<SignInManager<User>>>();
            _userManagerMock = _fixture.Freeze<Mock<UserManager<User>>>();
            _productServiceMock = _fixture.Freeze<Mock<IProductService>>();
            _productSizeServiceMock = _fixture.Freeze<Mock<IProductSizeService>>();
            _imageServiceMock = _fixture.Freeze<Mock<IImageService>>();
            _mapperMock = _fixture.Freeze<Mock<IMapper>>();

            _productController = new ProductController(
                _productServiceMock.Object,
                _mapperMock.Object,
                _imageServiceMock.Object,
                _productSizeServiceMock.Object,
                _signInManagerMock.Object,
                _userManagerMock.Object);
        }

        [Fact]
        public async void AddProductAsync_InvalidUserInput_ThrowsException()
        {
            // Arrange
            CreateProductDTO createProductDTO = null;

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await _productController.AddProductAsync(createProductDTO));
        }

        [Fact]
        public async void AddProductAsync_ValidInput_ReturnsOkResult()
        {
            // Arrange
            var userInput = _fixture.Create<CreateProductDTO>();

            _productServiceMock.Setup(x => x.AddProductAsync(userInput))
                .Verifiable();

            // Act
            var result = await _productController.AddProductAsync(userInput);

            // Assert
            _productServiceMock.Verify();
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async void RemoveProductAsync_WithIncorretId_ThrowsException()
        {
            // Arrange
            _productServiceMock.Setup(x => x.DeleteProductAsync(It.IsAny<int>()))
                .ThrowsAsync(new Exception());

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await _productController.RemoveProductAsync(It.IsAny<int>()));
        }

        [Fact]
        public async void RemoveProductAsync_WithCorrectId_ReturnsOkResult()
        {
            // Arrange
            _productServiceMock.Setup(x => x.DeleteProductAsync(It.IsAny<int>()))
                .Verifiable();

            // Act
            var result = await _productController.RemoveProductAsync(It.IsAny<int>());

            // Assert
            _productServiceMock.Verify();
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async void UpdateProductAsync_InvalidInput_ThrowsException()
        {
            // Arrange
            CreateProductDTO userInput = null;

            // Act % Assert
            await Assert.ThrowsAsync<Exception>(async () => await _productController.UpdateProductAsync(userInput));
        }

        [Fact]
        public async void UpdateProductAsync_ValidInput_ReturnsOkResult()
        {
            // Arrange
            var userInput = _fixture.Create<CreateProductDTO>();

            _productServiceMock.Setup(x => x.EditProductAsync(userInput))
                .Verifiable();

            // Act
            var result = await _productController.UpdateProductAsync(userInput);

            // Assert
            _productServiceMock.Verify();
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async void GetProductByIdAsync_ValidInput_ReturnsOkObjectResult()
        {
            // Arrange
            var product = _fixture.Create<Product>();
            var productResponseDTO = _fixture.Create<ProductResponseDTO>();

            _productServiceMock.Setup(x => x.GetProductByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(product);

            _mapperMock.Setup(x => x.Map<ProductResponseDTO>(product))
                .Returns(productResponseDTO);

            // Act
            var result = await _productController.GetProductByIdAsync(product.Id);
            var resultValue = TestHelpers.GetObjectResultContent<ProductResponseDTO>(result);

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(productResponseDTO, resultValue);
        }

        [Fact]
        public async void GetAllProductsAsync_ReturnsOkObjectResult()
        {
            // Arrange
            var products = _fixture.Build<Product>()
                .CreateMany(5)
                .AsEnumerable();

            var productsResponse = _fixture.Build<ProductResponseDTO>()
                .CreateMany(5)
                .ToList();

            _productServiceMock.Setup(x => x.GetAllProductsAsync())
                .ReturnsAsync(products);

            _mapperMock.Setup(x => x.Map<List<ProductResponseDTO>>(products))
                .Returns(productsResponse);

            // Act
            var result = await _productController.GetAllProductsAsync();
            var resultValue = TestHelpers.GetObjectResultContent<List<ProductResponseDTO>>(result);

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(productsResponse, resultValue);
        }

        [Theory, CustomAutoData]
        public async void GetAllProductByTypeAsync_ValidInput_ReturnsOkObjectResult(string type, int skipCount)
        {
            // Arrange
            var products = _fixture.Build<Product>()
                .CreateMany(5)
                .AsEnumerable();

            var response = _fixture.Build<ProductResponseDTO>()
                .CreateMany(5)
                .ToList();

            _productServiceMock.Setup(x => x.GetProductsByTypeAsync(type, skipCount))
                .ReturnsAsync(products);

            //_mapperMock.Setup(x => x.Map<ProductResponseDTO>(products))
            //    .Returns(response);

            // Act

            // Assert
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

using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using OnlineShop.Controllers;
using OnlineShop.DAL.Repository.Product;
using OnlineShop.Mapping.Models;
using OnlineShop.Models;
using OnlineShop.Models.Enums;
using OnlineShop.Services.Product;
using OnlineShop.Shared.DTO.ProductDTO;
using OnlineShop.UnitTests.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
            var resultValue = TestHelpers.GetObjectResultContent(result);

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
            var resultValue = TestHelpers.GetObjectResultContent(result);

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

            _mapperMock.Setup(x => x.Map<List<ProductResponseDTO>>(products))
                .Returns(response);

            // Act
            var result = await _productController.GetAllProductsByTypeAsync(type, skipCount);
            var resultValue = TestHelpers.GetObjectResultContent(result);

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(response, resultValue);
        }

        [Theory, CustomAutoData]
        public async void GetNewestProducts_ValidInput_ReturnsOkObjectResult(string type, int skipCount)
        {
            // Arrange
            var products = _fixture.Build<Product>()
                .CreateMany(5)
                .AsEnumerable();

            var response = _fixture.Build<ProductResponseDTO>()
                .CreateMany(5)
                .ToList();

            _productServiceMock.Setup(x => x.GetNewestProductsAsync(type, skipCount))
                .ReturnsAsync(products);

            _mapperMock.Setup(x => x.Map<List<ProductResponseDTO>>(products))
                .Returns(response);

            // Act
            var result = await _productController.GetNewestProductsAsync(type, skipCount);
            var resultValue = TestHelpers.GetObjectResultContent(result);

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(response, resultValue);
        }

        [Theory, CustomAutoData]
        public async void GetMostSoldProducts_ValidInput_ReturnsOkObjectResult(string type, int skipCount)
        {
            // Arrange
            var products = _fixture.Build<Product>()
                .CreateMany(5)
                .AsEnumerable();

            var response = _fixture.Build<ProductResponseDTO>()
                .CreateMany(5)
                .ToList();

            _productServiceMock.Setup(x => x.GetMostSoldProductsAsync(type, skipCount))
                .ReturnsAsync(products);

            _mapperMock.Setup(x => x.Map<List<ProductResponseDTO>>(products))
                .Returns(response);

            // Act
            var result = await _productController.GetMostSoldProductsAsync(type, skipCount);
            var resultValue = TestHelpers.GetObjectResultContent(result);

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(response, resultValue);
        }

        [Theory, CustomAutoData]
        public async void GetFilteredAndSortedProductsAsync_ValidInput_ReturnsOkObjectResult(ProductFilterDTO filter, SortType sortType, int skipCount)
        {
            // Arrange
            var products = _fixture.Build<Product>()
                .CreateMany(5)
                .AsEnumerable();

            var response = _fixture.Build<ProductResponseDTO>()
                .CreateMany(5)
                .ToList();

            _productServiceMock.Setup(x => x.GetFilteredAndSortedProductsAsync(filter, skipCount, sortType))
                .ReturnsAsync(products);

            _mapperMock.Setup(x => x.Map<List<ProductResponseDTO>>(products))
                .Returns(response);

            // Act
            var result = await _productController.GetFilteredAndSortedProductsAsync(filter, skipCount, sortType);
            var resultValue = TestHelpers.GetObjectResultContent(result);

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(response, resultValue);
        }

        [Fact]
        public async void GetAllProductSizes_ValidInput_ReturnsOkObjectResult()
        {
            // Arrange
            var sizes = _fixture.Build<ProductSizes>()
                .CreateMany(5)
                .AsEnumerable();

            var response = _fixture.Build<ProductSizesResponseDTO>()
                .CreateMany(5)
                .ToList();

            _productSizeServiceMock.Setup(x => x.GetAllProductSizesAsync())
                .ReturnsAsync(sizes);

            _mapperMock.Setup(x => x.Map<List<ProductSizesResponseDTO>>(sizes))
                .Returns(response);

            // Act
            var result = await _productController.GetAllProductSizesAsync();
            var resultValue = TestHelpers.GetObjectResultContent(result);

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(response, resultValue);
        }

        [Fact]
        public async void GetAllProductColors_ValidInput_ReturnsOkObjectResult()
        {
            // Arrange
            var colors = _fixture.Build<ProductColors>()
                .CreateMany(5)
                .AsEnumerable();

            var response = _fixture.Build<ProductColorsResponseDTO>()
                .CreateMany(5)
                .ToList();

            _productServiceMock.Setup(x => x.GetAllProductColorsAsync())
                .ReturnsAsync(colors);

            _mapperMock.Setup(x => x.Map<List<ProductColorsResponseDTO>>(colors))
                .Returns(response);

            // Act
            var result = await _productController.GetAllProductColorsAsync();
            var resultValue = TestHelpers.GetObjectResultContent(result);

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(response, resultValue);
        }

        [Fact]
        public async void GetAllUserFavouriteProducts_NotLoggedIn_ReturnsBadRequest()
        {
            // Arrange

            // Act
            var result = await _productController.GetAllUserFavouriteProductsAsync();

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Theory, CustomAutoData]
        public async void GetAllUserFavouriteProducts_LoggedIn_ReturnsOkObjectResult(string userId)
        {
            // Arrange
            SetUserClaims(userId);

            var products = _fixture.Build<Product>()
                .CreateMany(5)
                .AsEnumerable();

            var response = _fixture.Build<ProductResponseDTO>()
                .CreateMany(5)
                .ToList();

            _productServiceMock.Setup(x => x.GetAllUserFavouriteProductsAsync(userId))
                .ReturnsAsync(products);

            _mapperMock.Setup(x => x.Map<List<ProductResponseDTO>>(products))
                .Returns(response);

            // Act
            var result = await _productController.GetAllUserFavouriteProductsAsync();
            var resultValue = TestHelpers.GetObjectResultContent(result);

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(response, resultValue);
        }

        [Fact]
        public async void AddProductToUserFavourites_NotLoggedIn_ReturnsBadRequest()
        {
            // Act
            var result = await _productController.AddProductToUserFavouritesAsync(It.IsAny<int>());

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Theory, CustomAutoData]
        public async void AddProductToUserFavourites_LoggedIn_ReturnsOkResult(int productId, string userId)
        {
            // Arrange
            SetUserClaims(userId);
            _productServiceMock.Setup(x => x.AddProductToUserFavouritesAsync(userId, productId))
                .Verifiable();

            // Act
            var result = await _productController.AddProductToUserFavouritesAsync(productId);

            // Assert
            _productServiceMock.Verify();
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async void DeleteProductFromFavourites_NotLoggedIn_ReturnsBadRequest()
        {
            // Act
            var result = await _productController.DeleteProductFromFavouritesAsync(It.IsAny<int>());

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Theory, CustomAutoData]
        public async void DeleteProductFromFavourites_LoggedIn_ReturnsOkResult(int productId, string userId)
        {
            // Arrange
            SetUserClaims(userId);
            _productServiceMock.Setup(x => x.DeleteProductFromFavouriteAsync(userId, productId))
                .Verifiable();

            // Act
            var result = await _productController.DeleteProductFromFavouritesAsync(productId);

            // Assert
            _productServiceMock.Verify();
            Assert.IsType<OkResult>(result);
        }

        [Theory, CustomAutoData]
        public void HasMoreProduct_NoMore_ReturnsFalse(ProductFilterDTO filter, int skipCount, string sortType)
        {
            // Arrange
            _productServiceMock.Setup(x => x.HasMoreProducts(filter, skipCount, sortType))
                .Returns(false);

            // Act
            var result = _productController.HasMoreProducts(filter, skipCount, sortType);

            // Assert
            Assert.False(result.Value);
        }

        [Theory, CustomAutoData]
        public void HasMoreProduct_HasMore_ReturnsTrue(ProductFilterDTO filter, int skipCount, string sortType)
        {
            // Arrange
            _productServiceMock.Setup(x => x.HasMoreProducts(filter, skipCount, sortType))
                .Returns(true);

            // Act
            var result = _productController.HasMoreProducts(filter, skipCount, sortType);

            // Assert
            Assert.True(result.Value);
        }

        private void SetupFixture()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        private void SetUserClaims(string userId)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId)
            };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var claimsPrincipal = new ClaimsPrincipal(identity);
            _productController.ControllerContext.HttpContext = new DefaultHttpContext { User = claimsPrincipal };
        }
    }
}

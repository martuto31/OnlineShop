using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using OnlineShop.Controllers;
using OnlineShop.DAL.Data;
using OnlineShop.DAL.Repository.Product;
using OnlineShop.DAL.Repository.User;
using OnlineShop.Models;
using OnlineShop.Models.Enums;
using OnlineShop.Services.Product;
using OnlineShop.Services.User;
using OnlineShop.Shared.DTO.ProductDTO;
using OnlineShop.Shared.DTO.UserDTO;
using OnlineShop.UnitTests.Helpers;
using System.Runtime.CompilerServices;

namespace OnlineShop.UnitTests.Controllers
{
    public class UserControllerTests
    {
        private readonly Mock<SignInManager<User>> _signInManagerMock;
        private readonly Mock<UserManager<User>> _userManagerMock;
        private readonly Mock<RoleManager<Role>> _roleManagerMock;
        private readonly Mock<IJsonTokenService> _jsonTokenServiceMock;
        private readonly Mock<IEmailService> _emailServiceMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly UserController _userController;
        private IFixture _fixture;

        public UserControllerTests()
        {
            SetupFixture();

            _signInManagerMock = _fixture.Freeze<Mock<SignInManager<User>>>();
            _userManagerMock = _fixture.Freeze<Mock<UserManager<User>>>();
            _roleManagerMock = _fixture.Freeze<Mock<RoleManager<Role>>>();
            _jsonTokenServiceMock = _fixture.Freeze<Mock<IJsonTokenService>>();
            _emailServiceMock = _fixture.Freeze<Mock<IEmailService>>();
            _configurationMock = _fixture.Freeze<Mock<IConfiguration>>();

            _userController = new UserController(
                _signInManagerMock.Object,
                _userManagerMock.Object,
                _roleManagerMock.Object,
                _jsonTokenServiceMock.Object,
                _emailServiceMock.Object,
                _configurationMock.Object);
        }

        [Theory, CustomAutoData]
        public async Task Register_UsernameAlreadyExists_ReturnsBadRequest(string username)
        {
            // Arrange
            var existingUser = _fixture.Build<User>()
                .With(x => x.UserName, username)
                .Create();
            
            var registerDTO = _fixture.Build<RegisterDTO>()
                .With(x => x.Username, username)
                .Create();

            _userManagerMock.Setup(x => x.FindByNameAsync(username))
                .ReturnsAsync(existingUser);

            // Act
            var result = await _userController.RegisterAsync(registerDTO);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Theory, CustomAutoData]
        public async Task Register_EmailAlreadyExists_ReturnsBadRequest(string email)
        {
            // Arrange
            var existingUser = _fixture.Build<User>()
                .With(x => x.Email, email)
                .Create();

            var registerDTO = _fixture.Build<RegisterDTO>()
                .With(x => x.Email, email)
                .Create();

            _userManagerMock.Setup(x => x.FindByEmailAsync(email))
                .ReturnsAsync(existingUser);

            // Act
            var result = await _userController.RegisterAsync(registerDTO);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void Register_NoRole_ThrowsException()
        {
            // Arrange
            var registerDTO = _fixture.Create<RegisterDTO>();

            _userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((User)null);
            _userManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync((User)null);
            _roleManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync((Role)null);

            // Act & Assert
            var result = Assert.ThrowsAsync<Exception>(async () => await _userController.RegisterAsync(registerDTO));
        }

        [Fact]
        public async Task Register_ValidInput_ReturnsOkObjectResult()
        {
            // Arrange
            var registerDTO = _fixture.Create<RegisterDTO>();
            var role = _fixture.Create<Role>();

            _userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((User)null);
            _userManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync((User)null);
            _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            _userManagerMock.Setup(x => x.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            _roleManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(role);

            // Act
            var result = await _userController.RegisterAsync(registerDTO);

            // Assert
            Assert.IsType<OkObjectResult>(result);
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

using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Models;
using OnlineShop.Services.User;
using OnlineShop.Shared.DTO.UserDTO;

namespace OnlineShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IJsonTokenService jsonTokenService;
        private readonly IMapper mapper;

        public UserController(
            IUserService userService,
            IJsonTokenService jsonTokenService,
            IMapper mapper)
        {
            this.userService = userService;
            this.jsonTokenService = jsonTokenService;
            this.mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync([FromBody] UserDTO userInput)
        {
            if (userInput == null)
            {
                throw new Exception();
            }

            await userService.AddUserAsync(userInput);

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDTO userInput)
        {
            if (userInput == null)
            {
                throw new Exception();
            }

            var user = await userService.LoginAsync(userInput);

            if(user == null)
            {
                throw new Exception("Incorrect username or password!");
            }

            var token = jsonTokenService.GenerateToken(user);

            return Ok(token);
        }

        [HttpDelete("RemoveAccount")]
        public async Task<IActionResult> RemoveUserAsync(int id)
        {
            await userService.DeleteUserAsync(id);

            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetUserById")]
        public async Task<IActionResult> GetUserByIdAsync(int id)
        {
            await userService.GetUserByIdAsync(id);

            return Ok();
        }
    }
}

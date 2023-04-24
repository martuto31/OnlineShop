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

        public UserController(IUserService userService)
        {
            this.userService = userService;
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

        [HttpDelete]
        public async Task<IActionResult> RemoveUserAsync([FromBody] int id)
        {
            await userService.DeleteUserAsync(id);

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetUserByIdAsync([FromBody] int id)
        {
            await userService.GetUserByIdAsync(id);

            return Ok();
        }
    }
}

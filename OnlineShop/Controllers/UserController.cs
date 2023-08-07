using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Models;
using OnlineShop.Shared.DTO.UserDTO;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using OnlineShop.Services.User;

namespace OnlineShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IJsonTokenService _jsonTokenService;

        public UserController(SignInManager<User> signInManager, UserManager<User> userManager, RoleManager<Role> roleManager, IJsonTokenService jsonTokenService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _jsonTokenService = jsonTokenService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            if (!ModelState.IsValid)
            {
                // If the model state is not valid, return a bad request with validation errors
                var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
                return BadRequest(new { Errors = errors });
            }

            // Check if a user with the same username or email already exists
            if (await _userManager.FindByNameAsync(registerDTO.Username) != null)
            {
                ModelState.AddModelError("Username", "Потребителското име вече е заето.");
                var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
                return BadRequest(new { Errors = errors });
            }

            if (await _userManager.FindByEmailAsync(registerDTO.Email) != null)
            {
                ModelState.AddModelError("Email", "Вече съществува акаунт с въведения имейл.");
                var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
                return BadRequest(new { Errors = errors });
            }

            // Create a new IdentityUser with the provided information
            var user = new User
            {
                UserName = registerDTO.Username,
                Email = registerDTO.Email,
            };

            var userRole = await _roleManager.FindByNameAsync("User");

            if (userRole != null)
            {
                // Assign the role to the user
                user.RoleId = userRole.Id;
            }

            // Register the user with the UserManager
            var result = await _userManager.CreateAsync(user, registerDTO.Password);

            // Check if the "User" role exists
            var role = await _roleManager.FindByNameAsync("User");
            if (role != null)
            {
                // Assign the role to the user
                await _userManager.AddToRoleAsync(user, role.Name);
            }

            if (!result.Succeeded)
            {
                // If registration failed, return a bad request with the error messages
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
                return BadRequest(new { Errors = errors });
            }

            // Registration successful
            return Ok(new { Message = "Registration successful." });
        }

        [AllowAnonymous]
        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn(LoginDTO model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(model.Username);

                // Create claims for the user
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.UserName)
                };

                // Check if the user is an Admin and add the "Admin" claim
                if (await _userManager.IsInRoleAsync(user, "Admin"))
                {
                    claims.Add(new Claim(ClaimTypes.Role, "Admin"));
                }

                var token = _jsonTokenService.GenerateToken(user, claims);

                return Ok(new { Token = token });
            }

            // If sign-in fails, return a 401 Unauthorized response
            return Unauthorized();
        }

        [HttpPost("SignOut")]
        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();

            return Ok();
        }
    }
}

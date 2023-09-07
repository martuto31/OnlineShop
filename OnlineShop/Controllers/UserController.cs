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
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public UserController(SignInManager<User> signInManager,
                            UserManager<User> userManager, RoleManager<Role> roleManager,
                            IJsonTokenService jsonTokenService, 
                            IEmailService emailService,
                            IConfiguration configuration)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _jsonTokenService = jsonTokenService;
            _emailService = emailService;
            _configuration = configuration;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync(RegisterDTO registerDTO)
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
            return Ok(new { Message = "Успешна регистрация." });
        }

        [AllowAnonymous]
        [HttpPost("SignIn")]
        public async Task<IActionResult> SignInAsync(LoginDTO model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var jwt = GetToken(authClaims);
                var token = new JwtSecurityTokenHandler().WriteToken(jwt);

                return Ok(token);
            }
            return Unauthorized();
        }

        [HttpPost("SignOut")]
        public async Task<IActionResult> SignOutAsync()
        {
            await _signInManager.SignOutAsync();

            return Ok();
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPasswordAsync(PasswordResetDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return BadRequest("Няма регистриран акаунт с този имейл.");
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            // Send the token to the user's email
            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetLink = $"https://localhost:7260/api/reset-password?token={resetToken}";
            var emailMessage = $"To reset your password, click <a href='{resetLink}'>here</a>.";

            // Send the email
            await _emailService.SendEmailAsync(user.Email, "Password Reset", emailMessage);

            return Ok(new { Message = "Password reset link sent to your email." });
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }
    }
}

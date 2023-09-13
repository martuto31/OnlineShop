using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Models;
using OnlineShop.Services.Product;
using System.Security.Claims;

namespace OnlineShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly UserManager<User> _userManager;

        public OrderController(IOrderService orderService, UserManager<User> userManager)
        {
            _orderService = orderService;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrdersByUserId()
        {
            var userId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return BadRequest("Не сте влезли в акаунта си.");
            }

            var orders = _orderService.GetOrdersByUserIdAsync(userId);
            return Ok(orders);
        }
    }
}

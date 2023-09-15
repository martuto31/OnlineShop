using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Mapping.Models;
using OnlineShop.Models;
using OnlineShop.Services.Product;
using OnlineShop.Shared.DTO.OrderDTO;
using SQLitePCL;
using System.Security.Claims;

namespace OnlineShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IOrderService _orderService;
        private readonly UserManager<User> _userManager;

        public OrderController(IOrderService orderService, UserManager<User> userManager, IMapper mapper)
        {
            _orderService = orderService;
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpGet("GetOrder")]
        public async Task<ActionResult<OrderResponseDTO>> GetOrderByIdAsync(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);

            if(order == null)
            {
                return BadRequest("Няма такава поръчка.");
            }

            var response = _mapper.Map<OrderResponseDTO>(order);

            return Ok(response);
        }

        [Authorize]
        [HttpGet("GetOrdersByUserId")]
        public async Task<ActionResult<IEnumerable<OrderResponseDTO>>> GetOrdersByUserIdAsync()
        {
            var userId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return BadRequest("Не сте влезли в акаунта си.");
            }

            var orders = await _orderService.GetOrdersByUserIdAsync(userId);

            if (!orders.Any())
            {
                return NotFound("Акаунтът все още няма поръчки.");
            }

            var response = _mapper.Map<IEnumerable<OrderResponseDTO>>(orders);

            return Ok(response);
        }

        [Authorize]
        [HttpPost("AddOrder")]
        public async Task<IActionResult> AddOrderAsync([FromBody] CreateOrderDTO orderInput)
        {
            var userId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return BadRequest("Не сте влезли в акаунта си.");
            }

            await _orderService.AddOrderAsync(orderInput, userId);
            return Ok();
        }

        [HttpPost("SetDepartDate")]
        public async Task<IActionResult> SetDepartDateAsync(DateTime departDate, int orderId)
        {
            await _orderService.SetDepartDate(departDate, orderId);

            return Ok();
        }

        [HttpPost("SetDeliveryDate")]
        public async Task<IActionResult> SetDeliveryDateAsync(DateTime deliveryDate, int orderId)
        {
            await _orderService.SetDepartDate(deliveryDate, orderId);

            return Ok();
        }

        [HttpPost("SetReturnDate")]
        public async Task<IActionResult> SetReturnDateAsync(DateTime returnDate, int orderId)
        {
            await _orderService.SetDepartDate(returnDate, orderId);

            return Ok();
        }
    }
}

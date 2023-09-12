using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineShop.DAL.Repository.Product;
using OnlineShop.Models;
using OnlineShop.Shared.DTO.OrderDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Services.Product
{
    public class OrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly UserManager<Models.User> _userManager;

        public OrderService(IOrderRepository orderRepository, UserManager<Models.User> userManager)
        {
            _orderRepository = orderRepository;
            _userManager = userManager;
        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            var order = await _orderRepository.GetOrderByIdAsync(id);

            if (order == null)
            {
                throw new Exception("Order is null");
            }

            return order;
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string id)
        {
            var orders = await _orderRepository.GetOrdersByUserIdAsync(id).ToListAsync();

            return orders;
        }

        public async void AddOrderAsync(CreateOrderDTO orderDTO, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new Exception("Не съществува такъв акаунт.");
            }

            var order = new Order
            {
                User = user,
                UserId = userId,
                DeliveryDate = null,
                ReturnDate = null,
                ShipmentDepartDate = null,
                IsReturned = false,
                IsShipped = false,
                Products = orderDTO.Products
            };

            await _orderRepository.AddAsync(order);
            await _orderRepository.SaveChangesAsync();
        }
    }
}

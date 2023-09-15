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
    public class OrderService : IOrderService
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

            return order;
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string id)
        {
            var orders = await _orderRepository.GetOrdersByUserIdAsync(id).ToListAsync();

            return orders;
        }

        public async Task AddOrderAsync(CreateOrderDTO orderDTO, string userId)
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
                OrderCreatedOn = DateTime.UtcNow,
                DeliveryDate = null,
                ReturnDate = null,
                ShipmentDepartDate = null,
                IsReturned = false,
                IsShipped = false,
                Region = orderDTO.Region,
                City = orderDTO.City,
                PostalCode = orderDTO.PostalCode,
                Address = orderDTO.Address,
                PhoneNumber = orderDTO.PhoneNumber,
                Name = orderDTO.Name,
                Surname = orderDTO.Surname,
                OrderWeight = orderDTO.OrderWeight,
                OrderTotal = orderDTO.OrderTotal,
            };

            var productOrder = orderDTO.ProductsId.Select(id => new ProductOrder { OrderId = order.Id, ProductId = id}).ToList();
            order.Products = productOrder;

            await _orderRepository.AddAsync(order);
            await _orderRepository.SaveChangesAsync();
        }

        public async Task SetDepartDate(DateTime departDate, int orderId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);

            if(order == null)
            {
                throw new Exception("Не съществува такава поръчка.");
            }

            order.ShipmentDepartDate = departDate;

            _orderRepository.UpdateOrder(order);
            await _orderRepository.SaveChangesAsync();
        }

        public async Task SetDeliveryDate(DateTime deliveryDate, int orderId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);

            if (order == null)
            {
                throw new Exception("Не съществува такава поръчка.");
            }

            order.DeliveryDate = deliveryDate;

            _orderRepository.UpdateOrder(order);
            await _orderRepository.SaveChangesAsync();
        }

        public async Task SetReturnDate(DateTime returnDate, int orderId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);

            if (order == null)
            {
                throw new Exception("Не съществува такава поръчка.");
            }

            order.DeliveryDate = returnDate;

            _orderRepository.UpdateOrder(order);
            await _orderRepository.SaveChangesAsync();
        }
    }
}

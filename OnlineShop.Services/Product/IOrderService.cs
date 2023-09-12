using Microsoft.AspNetCore.Identity;
using OnlineShop.Models;
using OnlineShop.Shared.DTO.OrderDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Services.Product
{
    public interface IOrderService
    {
        void AddOrderAsync(CreateOrderDTO orderDTO, string userId);
        Task<Order> GetOrderByIdAsync(int id);
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string id);
        Task SetDepartDate(DateTime departDate, int orderId);
        Task SetDeliveryDate(DateTime deliveryDate, int orderId);
        Task SetReturnDate(DateTime returnDate, int orderId);
    }
}

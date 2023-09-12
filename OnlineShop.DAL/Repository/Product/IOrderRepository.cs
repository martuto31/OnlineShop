using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.DAL.Repository.Product
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<Order?> GetOrderByIdAsync(int id);
        IQueryable<Order> GetOrdersByUserIdAsync(string id);
        void UpdateOrder(Order order);
    }
}

﻿using Microsoft.EntityFrameworkCore;
using OnlineShop.DAL.Data;
using OnlineShop.Models;
using OnlineShop.Shared.DTO.OrderDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.DAL.Repository.Product
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext context): base(context) 
        {
        }

        public async Task<Order?> GetOrderByIdAsync(int id)
        {
            return await DbSet
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        public IQueryable<Order> GetOrdersByUserIdAsync(string id)
        {
            return DbSet
                .Include(x => x.User)
                .Where(x => x.UserId == id)
                .AsQueryable();
        }

        public void UpdateOrder(Order order)
        {
            DbSet.Update(order);
        }
    }
}

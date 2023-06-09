﻿using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.DAL.Repository.Product
{
    public interface IReviewRepository : IGenericRepository<Review>
    {
        Task AddReviewAsync(Review review);
        Task<IEnumerable<Review>> GetAllReviewsForProductAsync(int productId);
        Task<Review?> GetReviewByIdAsync(int id);
        void UpdateReview(Review review);
        void DeleteReview(Review review);
    }
}

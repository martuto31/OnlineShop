using Microsoft.EntityFrameworkCore;
using OnlineShop.DAL.Data;
using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.DAL.Repository.Product
{
    public class ReviewRepository : GenericRepository<Review>, IReviewRepository
    {
        public ReviewRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext) { }

        public async Task AddReviewAsync(Review review)
        {
            await _context.Reviews.AddAsync(review);
        }

        public void DeleteReview(Review review)
        {
            _context.Reviews.Remove(review);
        }

        public async Task<IEnumerable<Review>> GetAllReviewsForProductAsync(int productId)
        {
            return await DbSet
                .Where(x => x.ProductId == productId)
                .ToListAsync();
        }

        public async Task<Review?> GetReviewByIdAsync(int id)
        {
            return await DbSet
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        public void UpdateReview(Review review)
        {
            _context.Reviews.Update(review);
        }
    }
}

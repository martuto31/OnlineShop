using OnlineShop.Models;
using OnlineShop.Shared.DTO.ProductDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Services.Product
{
    public interface IReviewService
    {
        Task AddReviewAsync(ReviewDTO input);
        Task<IEnumerable<Review>> GetAllReviewsForProductAsync(int productId);
        Task<Review?> GetReviewByIdAsync(int id);
        Task UpdateReviewAsync(ReviewDTO input);
        Task DeleteReviewAsync(int id);
    }
}

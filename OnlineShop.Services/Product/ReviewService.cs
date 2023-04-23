using OnlineShop.DAL.Repository.Product;
using OnlineShop.Models;
using OnlineShop.Shared.DTO.ProductDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Services.Product
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository reviewRepository;

        public ReviewService(IReviewRepository reviewRepository)
        {
            this.reviewRepository = reviewRepository;
        }

        public async Task AddReviewAsync(ReviewDTO input)
        {
            var review = new Review()
            {
                Content = input.Content,
                PostenOn = input.PostenOn,
                ProductId = input.ProductId,
                UserId = input.UserId
            };

            await reviewRepository.AddReviewAsync(review);
            await reviewRepository.SaveChangesAsync();
        }

        public async Task DeleteReviewAsync(int id)
        {
            var review = await reviewRepository.GetReviewByIdAsync(id);

            if(review == null)
            {
                throw new Exception("Object should not be null.");
            }

            reviewRepository.DeleteReview(review);
            await reviewRepository.SaveChangesAsync();
        }

        public Task<IEnumerable<Review>> GetAllReviewsForProductAsync(int productId)
        {
            var reviews = reviewRepository.GetAllReviewsForProductAsync(productId);

            return reviews;
        }

        public async Task<Review?> GetReviewByIdAsync(int id)
        {
            var review = await reviewRepository.GetReviewByIdAsync(id);

            if(review == null)
            {
                throw new Exception("Object should not be null.");
            }

            return review;
        }

        public async Task UpdateReviewAsync(ReviewDTO input)
        {
            var review = await reviewRepository.GetReviewByIdAsync(input.Id);

            if(review == null)
            {
                throw new Exception("Object should not be null.");
            }

            review.Content = input.Content;

            reviewRepository.UpdateReview(review);
            await reviewRepository.SaveChangesAsync();
        }
    }
}

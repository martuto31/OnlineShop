using Microsoft.AspNetCore.Mvc;
using OnlineShop.Services.Product;
using OnlineShop.Shared.DTO.ProductDTO;

namespace OnlineShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService reviewService;

        public ReviewController(IReviewService reviewService)
        {
            this.reviewService = reviewService;
        }

        [HttpPost("AddReview")]
        public async Task<IActionResult> AddReviewAsync([FromBody] ReviewDTO userInput)
        {
            if(userInput == null)
            {
                throw new Exception();
            }

            await reviewService.AddReviewAsync(userInput);

            return Ok();
        }

        [HttpPut("UpdateReview")]
        public async Task<IActionResult> UpdateReviewAsync([FromBody] ReviewDTO userInput)
        {
            if (userInput == null)
            {
                throw new Exception();
            }

            await reviewService.UpdateReviewAsync(userInput);

            return Ok();
        }

        [HttpDelete("RemoveReview")]
        public async Task<IActionResult> RemoveReviewAsync(int id)
        {
            await reviewService.DeleteReviewAsync(id);

            return Ok();
        }

        [HttpGet("GetReviewById")]
        public async Task<IActionResult> GetReviewByIdAsync(int id)
        {
            await reviewService.GetReviewByIdAsync(id);

            return Ok();
        }

        [HttpGet("GetAllReviews")]
        public async Task<IActionResult> GetAllReviewsAsync(int productId)
        {
            await reviewService.GetAllReviewsForProductAsync(productId);

            return Ok();
        }
    }
}

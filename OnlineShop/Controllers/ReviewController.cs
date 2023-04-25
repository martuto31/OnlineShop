using AutoMapper;
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
        private readonly IMapper mapper;

        public ReviewController(IReviewService reviewService, IMapper mapper)
        {
            this.reviewService = reviewService;
            this.mapper = mapper;
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
        public async Task<ActionResult<ReviewDTO>> GetReviewByIdAsync(int id)
        {
            var review = await reviewService.GetReviewByIdAsync(id);

            var response = mapper.Map<ReviewDTO>(review);

            return Ok();
        }

        [HttpGet("GetAllReviews")]
        public async Task<ActionResult<List<ReviewDTO>>> GetAllReviewsAsync(int productId)
        {
            var reviews = await reviewService.GetAllReviewsForProductAsync(productId);

            var response = mapper.Map<List<ReviewDTO>>(reviews);

            return Ok(response);
        }
    }
}

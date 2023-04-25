using Microsoft.AspNetCore.Mvc;
using OnlineShop.Services.Product;
using OnlineShop.Services.User;
using OnlineShop.Shared.DTO.ProductDTO;
using OnlineShop.Shared.DTO.UserDTO;

namespace OnlineShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService productService;

        public ProductController(IProductService productService)
        {
            this.productService = productService;
        }

        [HttpPost("AddProduct")]
        public async Task<IActionResult> AddProductAsync([FromBody] ProductDTO userInput)
        {
            if (userInput == null)
            {
                throw new Exception();
            }

            await productService.AddProductAsync(userInput);

            return Ok();
        }

        [HttpDelete("RemoveProduct")]
        public async Task<IActionResult> RemoveProductAsync(int id)
        {
            await productService.DeleteProductAsync(id);

            return Ok();
        }

        [HttpPut("UpdateProduct")]
        public async Task<IActionResult> UpdateProductAsync([FromBody] ProductDTO userInput)
        {
            if (userInput == null)
            {
                throw new Exception();
            }

            await productService.EditProductAsync(userInput);

            return Ok();
        }

        [HttpGet("GetProductById")]
        public async Task<IActionResult> GetProductByIdAsync(int id)
        {
            await productService.GetProductByIdAsync(id);

            return Ok();
        }

        [HttpGet("GetAllProducts")]
        public async Task<IActionResult> GetAllProductsAsync()
        {
            await productService.GetAllProductsAsync();

            return Ok();
        }
    }
}

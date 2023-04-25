using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Mapping.Models;
using OnlineShop.Models;
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
        private readonly IMapper mapper;

        public ProductController(IProductService productService, IMapper mapper)
        {
            this.productService = productService;
            this.mapper = mapper;
        }

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
        [HttpDelete("RemoveProduct")]
        public async Task<IActionResult> RemoveProductAsync(int id)
        {
            await productService.DeleteProductAsync(id);

            return Ok();
        }

        [Authorize(Roles = "Admin")]
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
        public async Task<ActionResult<ProductResponseDTO>> GetProductByIdAsync(int id)
        {
            var product = await productService.GetProductByIdAsync(id);

            var response = mapper.Map<ProductResponseDTO>(product);

            return Ok(response);
        }

        [HttpGet("GetAllProducts")]
        public async Task<ActionResult<List<ProductResponseDTO>>> GetAllProductsAsync()
        {
            var products = await productService.GetAllProductsAsync();

            var response = mapper.Map<List<ProductResponseDTO>>(products);

            return Ok(response);
        }
    }
}

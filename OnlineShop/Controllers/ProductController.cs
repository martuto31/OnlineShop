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
        private readonly IImageService imageService;
        private readonly IMapper mapper;

        public ProductController(IProductService productService, IMapper mapper, IImageService imageService)
        {
            this.productService = productService;
            this.mapper = mapper;
            this.imageService = imageService;
        }

        //[Authorize(Roles = "Admin")]
        [HttpPost("AddProduct")]
        public async Task<IActionResult> AddProductAsync([FromForm] CreateProductDTO userInput)
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
        public async Task<IActionResult> UpdateProductAsync([FromBody] CreateProductDTO userInput)
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

            var productResponse = mapper.Map<ProductResponseDTO>(product);
            var images = await imageService.GetAllImagesForProductAsync(product.Id);

            // Convert the binary byte array to base64
            foreach (var image in images)
            {
                productResponse.PicturesData.Add(Convert.ToBase64String(image.Image));
            }

            return Ok(productResponse);
        }

        [HttpGet("GetAllProducts")]
        public async Task<ActionResult<List<ProductResponseDTO>>> GetAllProductsAsync()
        {
            var products = await productService.GetAllProductsAsync();

            var response = mapper.Map<List<ProductResponseDTO>>(products);

            return Ok(response);
        }

        [HttpGet("GetProductsByType")]
        public async Task<ActionResult<List<ProductResponseDTO>>> GetAllProductsByTypeAsync(string type)
        {
            var products = await productService.GetProductsByTypeAsync(type);

            var response = new List<ProductResponseDTO>();
            
            // Refactor
            foreach (var product in products)
            {
                var productResponse = mapper.Map<ProductResponseDTO>(product);
                var images = await imageService.GetAllImagesForProductAsync(product.Id);

                // Convert the binary byte array to base64
                foreach (var image in images)
                {
                    productResponse.PicturesData.Add(Convert.ToBase64String(image.Image));
                }

                response.Add(productResponse);
            }

            return Ok(response);
        }
    }
}

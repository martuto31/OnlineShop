﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Mapping.Models;
using OnlineShop.Models;
using OnlineShop.Models.Enums;
using OnlineShop.Services.Product;
using OnlineShop.Services.User;
using OnlineShop.Shared.DTO.ProductDTO;
using OnlineShop.Shared.DTO.UserDTO;
using System.Security.Claims;

namespace OnlineShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;
        private readonly IProductService productService;
        private readonly IProductSizeService productSizeService;
        private readonly IImageService imageService;
        private readonly IMapper mapper;

        public ProductController(
            IProductService productService,
            IMapper mapper,
            IImageService imageService,
            IProductSizeService productSizeService,
            SignInManager<User> signInManager,
            UserManager<User> userManager)
        {
            this.productService = productService;
            this.mapper = mapper;
            this.imageService = imageService;
            this.productSizeService = productSizeService;
            this.signInManager = signInManager;
            this.userManager = userManager;
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
        public async Task<IActionResult> UpdateProductAsync([FromForm] CreateProductDTO userInput)
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
        public async Task<ActionResult<List<ProductResponseDTO>>> GetAllProductsByTypeAsync(string type, int skipCount)
        {
            var products = await productService.GetProductsByTypeAsync(type, skipCount);

            var response = mapper.Map<List<ProductResponseDTO>>(products);

            return Ok(response);
        }

        [HttpGet("GetNewestProducts")]
        public async Task<ActionResult<List<ProductResponseDTO>>> GetNewestProductsAsync(string type, int skipCount)
        {
            var products = await productService.GetNewestProductsAsync(type, skipCount);

            var response = mapper.Map<List<ProductResponseDTO>>(products);

            return Ok(response);
        }

        [HttpGet("GetMostSoldProducts")]
        public async Task<ActionResult<List<ProductResponseDTO>>> GetMostSoldProductsAsync(string type, int skipCount)
        {
            var products = await productService.GetMostSoldProductsAsync(type, skipCount);

            var response = mapper.Map<List<ProductResponseDTO>>(products);

            return Ok(response);
        }

        [HttpPost("GetFilteredAndSortedProductsAsync")]
        public async Task<ActionResult<List<ProductResponseDTO>>> GetFilteredAndSortedProductsAsync([FromBody] ProductFilterDTO filter, int skipCount, SortType sortType)
        {
            var products = await productService.GetFilteredAndSortedProductsAsync(filter, skipCount, sortType);

            var response = mapper.Map<List<ProductResponseDTO>>(products);

            return Ok(response);
        }

        [HttpGet("GetAllProductSizes")]
        public async Task<ActionResult<List<ProductSizesResponseDTO>>> GetAllProductSizesAsync()
        {
            var sizes = await productSizeService.GetAllProductSizesAsync();

            var response = mapper.Map<List<ProductSizesResponseDTO>>(sizes);

            return Ok(response);
        }

        [HttpGet("GetAllProductColors")]
        public async Task<ActionResult<List<ProductColorsResponseDTO>>> GetAllProductColorsAsync()
        {
            var colors = await productService.GetAllProductColorsAsync();

            var response = mapper.Map<List<ProductColorsResponseDTO>>(colors);

            return Ok(response);
        }

        [HttpGet("GetAllUserFavouriteProducts")]
        public async Task<ActionResult<List<ProductResponseDTO>>> GetAllUserFavouriteProductsAsync()
        {
            var userId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return BadRequest("Не сте влезли в акаунта си.");
            }

            var products = await productService.GetAllUserFavouriteProductsAsync(userId);

           var response = mapper.Map<List<ProductResponseDTO>>(products);

            return Ok(response);
        }

        [HttpPost("AddProductToUserFavourites")]
        public async Task<IActionResult> AddProductToUserFavouritesAsync(int productId)
        {
            var userId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return BadRequest("Не сте влезли в акаунта си.");
            }

            await productService.AddProductToUserFavouritesAsync(userId, productId);

            return Ok();
        }

        [HttpDelete("DeleteProductFromFavourites")]
        public async Task<IActionResult> DeleteProductFromFavouritesAsync(int productId)
        {
            var userId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return BadRequest("Не сте влезли в акаунта си.");
            }

            await productService.DeleteProductFromFavouriteAsync(userId, productId);

            return Ok();
        }

        [HttpPost("HasMoreProducts")]
        public ActionResult<bool> HasMoreProducts([FromBody] ProductFilterDTO filter, int skipCount, string sortType)
        {
            return productService.HasMoreProducts(filter, skipCount, sortType);
        }
    }
}

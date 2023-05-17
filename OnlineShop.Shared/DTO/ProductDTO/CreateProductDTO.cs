using Microsoft.AspNetCore.Http;
using OnlineShop.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Shared.DTO.ProductDTO
{
    public class CreateProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile Image { get; set; }
        public double Price { get; set; }

        public ProductTarget ProductTarget { get; set; }
        public ProductType ProductType { get; set; }
    }
}

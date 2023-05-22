using Microsoft.AspNetCore.Http;
using OnlineShop.Models;
using OnlineShop.Models.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public IList<IFormFile> Images { get; set; }
        public double Price { get; set; }

        public ProductTarget ProductTarget { get; set; }
        public ProductType ProductType { get; set; }

        public ICollection<ProductSizes> ProductSizes{ get; set; }
        public ICollection<ProductColors> ProductColors{ get; set; }
    }
}

using OnlineShop.Models;
using OnlineShop.Models.Enums;

namespace OnlineShop.Shared.DTO.ProductDTO
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public ProductTarget ProductTarget { get; set; }
        public ProductType ProductType { get; set; }

        public ICollection<Review>? Reviews { get; set; }
    }
}

using OnlineShop.Models.Enums;

namespace OnlineShop.Mapping.Models
{
    public class ProductResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public ProductTarget ProductTarget { get; set; }
        public ProductType ProductType { get; set; }
    }
}

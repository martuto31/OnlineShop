using OnlineShop.Models;

namespace OnlineShop.Shared.DTO.ProductDTO
{
    public class ReviewDTO
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime PostenOn { get; set; }

        public int ProductId { get; set; }

        public int UserId { get; set; }
    }
}

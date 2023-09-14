using OnlineShop.Models;

namespace OnlineShop.Mapping.Models
{
    public class OrderResponseDTO
    {
        public DateTime? ShipmentDepartDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public bool IsShipped { get; set; }
        public bool IsReturned { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}

using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Shared.DTO.OrderDTO
{
    public class CreateOrderDTO
    {
        public DateTime? ShipmentDepartDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public bool IsShipped { get; set; }
        public bool IsReturned { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }
        public ICollection<ProductOrder> Products { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime ShipmentDepartDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public bool IsShipped { get; set; }
        public bool IsReturned { get; set; }

        public ICollection<ProductOrder> Products { get; set; }
    }
}

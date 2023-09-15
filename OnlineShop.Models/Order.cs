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
        public double OrderTotal { get; set; }
        public double OrderWeight { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime OrderCreatedOn { get; set; }
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

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
        public ICollection<int> ProductsId { get; set; }
    }
}

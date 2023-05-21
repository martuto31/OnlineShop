using OnlineShop.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OnlineShop.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public ProductTarget ProductTarget { get; set; }
        public ProductType ProductType { get; set; }
        public ProductColor ProductColor { get; set; }
        public ProductSize ProductSize { get; set; }

        public ICollection<ImageUri> Pictures { get; set; }

        public  ICollection<Review> Reviews { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Models
{
    public class ProductsWithSizes
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int ProductSizesId { get; set; }
        public ProductSizes ProductSizes { get; set; }
    }
}

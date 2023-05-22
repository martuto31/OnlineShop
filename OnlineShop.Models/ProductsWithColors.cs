using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Models
{
    public class ProductsWithColors
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int ProductColorsId { get; set; }
        public ProductColors ProductColors { get; set; }
    }
}

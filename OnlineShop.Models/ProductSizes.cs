using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Models
{
    public class ProductSizes
    {
        public int Id { get; set; }
        [Required]
        public int Size { get; set; }

        public Product Product { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string Content { get; set; }

        public DateTime PostenOn { get; set; }

        public int ProductId { get; set;}
        public Product Product { get; set; }
        public User User { get; set; }
    }
}

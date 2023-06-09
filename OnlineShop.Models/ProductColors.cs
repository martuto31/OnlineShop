﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Models
{
    public class ProductColors
    {
        public int Id { get; set; }
        [Required]
        public string Color { get; set; }

        public ICollection<ProductsWithColors>? ProductsWithColors { get; set; }
    }
}

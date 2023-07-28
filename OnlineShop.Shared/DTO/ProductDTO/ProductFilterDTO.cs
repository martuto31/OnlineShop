using Microsoft.Identity.Client;
using OnlineShop.Models;
using OnlineShop.Models.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Shared.DTO.ProductDTO
{
    public class ProductFilterDTO
    {
        public IEnumerable<LightIntensity> LightIntensities { get; set; }
        public IEnumerable<ProductSize> Sizes { get; set; }
        public bool PetCompatibility { get; set; }
        public bool AirPurifiable { get; set; }
        public IEnumerable<ProductColor> Colors { get; set; }
        public IEnumerable<GrowDifficulty> GrowDifficulties { get; set; }
        public ProductType productType { get; set; }
    }
}

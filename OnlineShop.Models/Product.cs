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
        public int Sales { get; set; }
        public string Name { get; set; }
        public string CommonName { get; set; }
        public string BotanicalName { get; set; }
        public string Description { get; set; }
        public string AdditionalDescription { get; set; }
        public double Price { get; set; }
        public DateTime CreatedOn { get; set; }
        public GrowDifficulty GrowDifficulty { get; set; }
        public LightIntensity LightIntensity { get; set; }
        public bool PetCompatibility {  get; set; }
        public bool AirPurify { get; set; }

        public ProductType ProductType { get; set; }

        public ICollection<ProductsWithColors> ProductsWithColors { get; set; }
        public ICollection<ProductsWithSizes> ProductsWithSizes { get; set; }

        public ICollection<ImageUri> Pictures { get; set; }
        public ICollection<Review> Reviews { get; set; }
    }
}

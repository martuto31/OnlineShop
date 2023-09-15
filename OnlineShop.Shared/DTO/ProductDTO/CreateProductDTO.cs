using Microsoft.AspNetCore.Http;
using OnlineShop.Models;
using OnlineShop.Models.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Shared.DTO.ProductDTO
{
    public class CreateProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CommonName { get; set; }
        public string BotanicalName { get; set; }
        public string AdditionalDescription { get; set; }
        public string Description { get; set; }
        public IList<IFormFile> Images { get; set; }
        public double Price { get; set; }
        public double Weight { get; set; }
        public LightIntensity LightIntensity { get; set; }
        public GrowDifficulty GrowDifficulty { get; set; }
        public bool PetCompatibility { get; set; }
        public bool AirPurify { get; set; }

        public ProductType ProductType { get; set; }

        public int[] ProductSizes{ get; set; }
        public int[] ProductColors{ get; set; }
    }
}

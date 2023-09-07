using OnlineShop.Models;
using OnlineShop.Models.Enums;

namespace OnlineShop.Mapping.Models
{
    public class ProductResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CommonName { get; set; }
        public string BotanicalName { get; set; }
        public string AdditionalDescription { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string GrowDifficulty { get; set; }
        public string LightIntensity { get; set; }
        public string ProductType { get; set; }
        public bool PetCompatibility { get; set; }
        public bool AirPurify { get; set; }
        public List<string> PicturesData { get; set; } = new List<string>();

        public List<string> ProductSizes { get; set; } = new List<string>();
        public List<string> ProductColors { get; set; } = new List<string>();

    }
}

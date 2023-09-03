using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OnlineShop.DAL.Repository.Product;
using OnlineShop.Models;
using SendGrid;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace OnlineShop.Services.Product
{
    public class ImageService : IImageService
    {
        private readonly IImageRepository imageRepository;

        public ImageService(IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }

        public static byte[] CompressAndResizeImage(byte[] imageBytes, int targetWidth, int targetHeight)
        {
            // Load the image from the byte array
            using (Image image = Image.Load(imageBytes))
            {
                // Resize the image to the target dimensions while maintaining the aspect ratio
                image.Mutate(x => x.Resize(new ResizeOptions
                {
                    Size = new Size(targetWidth, targetHeight),
                    Mode = ResizeMode.Max
                }));

                // Create a new MemoryStream to store the compressed image
                using (MemoryStream outputStream = new MemoryStream())
                {
                    // Save the resized image as a JPEG with specified quality
                    image.Save(outputStream, new JpegEncoder { Quality = 80 });

                    // Return the compressed image data as a byte array
                    return outputStream.ToArray();
                }
            }
        }

        public async Task<IEnumerable<string>> GetAllImagesForProductAsBase64Async(int productId)
        {
            var imageFiles = await imageRepository.GetAllImagesForProductAsync(productId).ToListAsync();

            var images = new List<string>();
            foreach (var imageFile in imageFiles)
            {
                images.Add(Convert.ToBase64String(imageFile.Image));
            }

            return images;
        }

        public List<ImageUri> GetImageFiles(IList<IFormFile> images)
        {
            var imageFiles = new List<ImageUri>();

            if (images != null && images.Count > 0)
            {
                foreach (var imageFile in images)
                {
                    byte[] img = ConvertIFormFileToByteArray(imageFile);
                    img = ImageService.CompressAndResizeImage(img, 400, 400);
                    var image = new ImageUri()
                    {
                        Image = img,
                    };
                    imageFiles.Add(image);
                }
            }

            return imageFiles;
        }

        private byte[] ConvertIFormFileToByteArray(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                file.CopyToAsync(memoryStream).GetAwaiter().GetResult();
                return memoryStream.ToArray();
            }
        }
    }
}

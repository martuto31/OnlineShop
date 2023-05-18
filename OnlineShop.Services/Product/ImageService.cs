using SixLabors.ImageSharp.Formats.Jpeg;

namespace OnlineShop.Services.Product
{
    public class ImageService
    {
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
    }
}

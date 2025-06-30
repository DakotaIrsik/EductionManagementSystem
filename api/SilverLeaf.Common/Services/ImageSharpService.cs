using SilverLeaf.Common.Models.Images.Interfaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;
using System.IO;

namespace SilverLeaf.Common.Services
{
    public interface IImageSharpService
    {
        void Create(byte[] imageData, string path, string fileName, IApplicationImage image);
        void CreateWithWatermark(byte[] imageData, string path, string fileName, IApplicationImage image);
    }
    public class ImageSharpService : IImageSharpService
    {
        public void Create(byte[] imageData, string path, string fileName, IApplicationImage image)
        {
            Directory.CreateDirectory($"{path}/{image.Folder}");
            using (Image<Rgba32> img = Image.Load(imageData))
            {
                if (image.WidthInPixels > 0 && image.HeightInPixels > 0)
                {
                    img.Mutate(x => x.Resize(image.WidthInPixels ?? img.Width, image.HeightInPixels ?? img.Height));
                }
                img.Save($"{path}/{image.Folder}/{fileName}");
            }
        }

        public void CreateWithWatermark(byte[] imageData, string path, string fileName, IApplicationImage image)
        {
            var waterMarkFolder = $"{path}/{image.Folder}/{image.Watermark.Folder}";
            Directory.CreateDirectory(waterMarkFolder);

            using (Image<Rgba32> img = Image.Load(imageData))
            {
                Image<Rgba32> waterMark = ResizeWatermark(Image.Load(image.Watermark.ImageData),
                    image.WidthInPixels ?? img.Width,
                    image.HeightInPixels ?? img.Height);

                int x = (image.WidthInPixels / 2 ?? img.Width / 2);
                int y = (image.HeightInPixels / 2 ?? img.Height / 2);

                if (image.WidthInPixels > 0 && image.HeightInPixels > 0)
                {
                    img.Mutate(img2 => img2.Resize(image.WidthInPixels ?? img.Width, image.HeightInPixels ?? img.Height));
                }
                img.Mutate(i => i.DrawImage(waterMark, new Point(x, y), 0.5f));
                img.Save($"{waterMarkFolder}/{fileName}"); // Automatic encoder selected based on extension.
            }
        }

        private Image<Rgba32> ResizeWatermark(Image<Rgba32> originalWatermark, int imageHeight, int imageWidth)
        {
            originalWatermark.Mutate(img => img.Resize(imageHeight / 4, imageWidth / 4));
            return originalWatermark;
        }
    }
}

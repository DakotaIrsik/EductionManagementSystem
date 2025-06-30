using SilverLeaf.Common.Constants;
using SilverLeaf.Common.Models.Images.Interfaces;
using System.IO;

namespace SilverLeaf.Common.Models.Images.Watermarks
{
    public class Watermark : IWatermark
    {
        public byte[] ImageData { get; set; }
        public string Folder => nameof(Watermark);
        public Watermark()
        {
            ImageData = File.ReadAllBytes($"{General.SeedDataFolder}/{nameof(Watermark)}.png");
        }
    }
}

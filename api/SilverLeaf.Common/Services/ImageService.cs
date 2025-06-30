using Microsoft.Extensions.Options;
using System.IO;
using static SilverLeaf.Common.Constants.ImageTypes;

namespace SilverLeaf.Common.Services
{
    public interface IImageService
    {
        string GetImagePath(string applicationName, string imageType);
        string StockOriginal(bool withWatermark);
    }

    public class ImageService : IImageService
    {
        private readonly AppSettings _settings;
        public ImageService(IOptions<AppSettings> settings)
        {
            _settings = settings.Value;
        }

        public string StockOriginal(bool withWaterMark)
        {
            return GetStockPhotoPath(nameof(StockOriginal).Replace("Stock", ""), true);
        }

        private string GetStockPhotoPath(string imageType, bool withWatermark)
        {
            var imagePath = GetImagePath(_settings.Name, SubTypes.Profile);
            return $"{_settings.Url}{_settings.StaticFileAlias}/{SubTypes.Profile}/{imageType}/" + ((withWatermark) ? "watermark/" : "") + "stock-profile.jpg";
        }

        public string GetImagePath(string applicationName, string imageType)
        {
            var dir = $"{_settings.StaticFilePath}/{imageType}";
            Directory.CreateDirectory(dir);
            return dir;
        }

    }
}

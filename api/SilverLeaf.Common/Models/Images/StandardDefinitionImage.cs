using SilverLeaf.Common.Models.Images.Interfaces;
using SilverLeaf.Common.Services;
using System.Threading.Tasks;

namespace SilverLeaf.Common.Models.Images
{
    public class StandardDefinition : IApplicationImage
    {
        private readonly IImageSharpService _imageSharpService;

        public StandardDefinition(IImageSharpService imageSharpService, IWatermark watermark)
        {
            _imageSharpService = imageSharpService;
            Watermark = watermark;
        }

        public IWatermark Watermark { get; set; }
        public int? HeightInPixels => 375;
        public int? WidthInPixels => 500;
        public string Folder => nameof(StandardDefinition);
        public byte[] ImageData { get; set; }

        public async Task Create(string path, string fileName)
        {
            await Task.Run(() => _imageSharpService.Create(ImageData, path, fileName, this)).ConfigureAwait(false);
            await Task.Run(() => _imageSharpService.CreateWithWatermark(ImageData, path, fileName, this)).ConfigureAwait(false);
        }
    }
}

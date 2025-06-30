using System.Threading.Tasks;

namespace SilverLeaf.Common.Models.Images.Interfaces
{
    public interface IApplicationImage
    {
        IWatermark Watermark { get; }
        string Folder { get; }
        int? HeightInPixels { get; }
        int? WidthInPixels { get; }
        byte[] ImageData { get; set; }
        Task Create(string path, string fileName);
    }
}

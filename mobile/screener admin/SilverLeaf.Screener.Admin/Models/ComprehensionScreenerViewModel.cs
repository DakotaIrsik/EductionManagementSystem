using SilverLeaf.Entities.Models;
using System.IO;
using Xamarin.Forms;

namespace SilverLeaf.Screener.Admin.Models
{
    public class ComprehensionScreenerViewModel : ComprehensionScreenerDTO
    {
        public ImageSource LocalImage => ImageSource.FromResource($"SilverLeaf.Screener.Resources.Images.{new FileInfo(Image).Name}");
        public ImageSource SecondLocalImage => ImageSource.FromResource($"SilverLeaf.Screener.Resources.Images.{new FileInfo(SecondImage).Name}");
        public double FontSize
        {
            get
            {
                return 40;
            }
        }
    }

    public class ComprehensionScreenerSectionViewModel
    {
        public ComprehensionScreenerSectionViewModel(ImageSource localImage, string preface)
        {
            LocalImage = localImage;
            Preface = preface;
        }

        public ImageSource LocalImage { get; set; }

        public string Preface { get; set; }

    }
}

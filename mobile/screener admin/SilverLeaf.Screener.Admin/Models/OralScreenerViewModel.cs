using SilverLeaf.Entities.Models;
using System.IO;
using Xamarin.Forms;

namespace SilverLeaf.Screener.Admin.Models
{
    public class OralScreenerViewModel : OralScreenerDTO
    {
        public ImageSource LocalImage => ImageSource.FromResource($"SilverLeaf.Screener.Resources.Images.{new FileInfo(Image).Name}");

        public double FontSize
        {
            get
            {
                return 40;
            }
        }
    }
}

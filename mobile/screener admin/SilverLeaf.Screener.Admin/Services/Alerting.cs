using SilverLeaf.Screener.Admin.Resources;
using SilverLeaf.Screener.Admin.Services.Interfaces;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SilverLeaf.Screener.Admin.Services
{
    public class Alerting : IAlerting
    {
        public Page Page => Application.Current.MainPage;

        public async Task Show(string title, string message, string button)
        {
            await Page.DisplayAlert(title, message, button);
        }

        public async Task<bool> Show(string title, string message, string buttonPositive, string buttonNegative)
        {
            return await Page.DisplayAlert(title, message, buttonPositive, buttonNegative);
        }

        public async Task ShowDefaultWarning(string message)
        {
            await Show(Lang.Warning, message, Lang.OK);
        }
    }
}

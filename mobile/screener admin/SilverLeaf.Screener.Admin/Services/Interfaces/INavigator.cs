using System.Threading.Tasks;
using Xamarin.Forms;

namespace SilverLeaf.Screener.Admin.Services.Interfaces
{
    public interface INavigator : INavigation
    {
        INavigation Nav { get; set; }
        void ShowLogin();

        /// <summary>
        /// Pop all modals off of the stack. Animate popping the top modal by default.
        /// </summary>
        /// <param name="animated">Whether nor not the top modal should animate while popping.</param>
        Task PopModalStackAsync(bool animated = true);
    }
}

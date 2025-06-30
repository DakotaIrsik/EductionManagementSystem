using Autofac;
using SilverLeaf.Screener.Admin.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SilverLeaf.Screener.Admin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        private readonly SettingsPageViewModel _viewModel;
        public SettingsPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = App.Container.Resolve<SettingsPageViewModel>();
        }
    }
}
using Autofac;
using SilverLeaf.Screener.Admin.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SilverLeaf.Screener.Admin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScreenerRegistrationPage : ContentPage
    {
        private readonly ScreenerRegistrationPageViewModel _viewModel;
        public ScreenerRegistrationPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = App.Container.Resolve<ScreenerRegistrationPageViewModel>();
        }
    }
}
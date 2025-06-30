using Autofac;
using SilverLeaf.Screener.Admin.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SilverLeaf.Screener.Admin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        private readonly LoginPageViewModel _viewModel;
        public LoginPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = App.Container.Resolve<LoginPageViewModel>();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.BlockBack = true;
            _viewModel.OnAppearing();
            var htmlSource = new HtmlWebViewSource();
        }

        protected override void OnDisappearing()
        {
            App.BlockBack = false;
            base.OnDisappearing();
        }
    }
}
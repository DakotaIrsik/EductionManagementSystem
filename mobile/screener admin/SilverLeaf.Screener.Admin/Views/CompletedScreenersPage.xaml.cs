using Autofac;
using SilverLeaf.Screener.Admin.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SilverLeaf.Screener.Admin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CompletedScreenersPage : ContentPage
    {
        private readonly CompletedScreenerPageViewModel _viewModel;
        public CompletedScreenersPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = App.Container.Resolve<CompletedScreenerPageViewModel>();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.OnAppearing().ConfigureAwait(false);
        }
    }
}
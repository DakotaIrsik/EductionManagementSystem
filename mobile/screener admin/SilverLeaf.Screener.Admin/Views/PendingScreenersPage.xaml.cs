using Autofac;
using SilverLeaf.Screener.Admin.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SilverLeaf.Screener.Admin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PendingScreenersPage : ContentPage
    {
        private readonly PendingScreenersPageViewModel _viewModel;
        public PendingScreenersPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = App.Container.Resolve<PendingScreenersPageViewModel>();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.OnAppearing().ConfigureAwait(false);
        }
    }
}
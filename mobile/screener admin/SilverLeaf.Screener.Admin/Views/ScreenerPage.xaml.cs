using Autofac;
using SilverLeaf.Entities.ViewModels.Requests.Responses;
using SilverLeaf.Screener.Admin.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SilverLeaf.Screener.Admin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScreenerPage : ContentPage
    {
        private readonly ScreenerPageViewModel _viewModel;
        public ScreenerPage(ScreenerSummaryResponse screenerSummary)
        {
            InitializeComponent();
            BindingContext = _viewModel = App.Container.Resolve<ScreenerPageViewModel>();
            _viewModel.ScreenerSummary = screenerSummary;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}
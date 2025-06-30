using Autofac;
using SilverLeaf.Screener.Admin.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SilverLeaf.Screener.Admin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OralScreenerQuestionPage : ContentPage
    {
        private readonly OralScreenerQuestionPageViewModel _viewModel;
        public OralScreenerQuestionPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = App.Container.Resolve<OralScreenerQuestionPageViewModel>();
        }

        //protected override async void OnDisappearing()
        //{
        //    base.OnDisappearing();
        //    await _viewModel.OnDisappearing().ConfigureAwait(false);
        //}

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}
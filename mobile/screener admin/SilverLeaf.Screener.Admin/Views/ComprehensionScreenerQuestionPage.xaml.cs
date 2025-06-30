using Autofac;
using SilverLeaf.Screener.Admin.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SilverLeaf.Screener.Admin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ComprehensionScreenerQuestionPage : ContentPage
    {
        private readonly ComprehensionScreenerQuestionPageViewModel _viewModel;
        public ComprehensionScreenerQuestionPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = App.Container.Resolve<ComprehensionScreenerQuestionPageViewModel>();
        }

        public ComprehensionScreenerQuestionPage(ComprehensionScreenerQuestionPageViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm = App.Container.Resolve<ComprehensionScreenerQuestionPageViewModel>();
            _viewModel = vm;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.OnAppearing().ConfigureAwait(false);
        }
    }
}
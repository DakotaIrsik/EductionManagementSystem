using Autofac;
using SilverLeaf.Screener.Admin.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SilverLeaf.Screener.Admin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PhonicsScreenerQuestionPage : ContentPage
    {
        private readonly PhonicsScreenerQuestionPageViewModel _viewModel;
        public PhonicsScreenerQuestionPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = App.Container.Resolve<PhonicsScreenerQuestionPageViewModel>();
        }

        public PhonicsScreenerQuestionPage(PhonicsScreenerQuestionPageViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm = App.Container.Resolve<PhonicsScreenerQuestionPageViewModel>();
            _viewModel = vm;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.OnAppearing().ConfigureAwait(false);
        }
    }
}
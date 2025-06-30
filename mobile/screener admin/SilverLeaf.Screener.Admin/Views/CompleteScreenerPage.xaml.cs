using Autofac;
using SilverLeaf.Screener.Admin.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SilverLeaf.Screener.Admin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CompleteScreenerPage : ContentPage
    {
        private readonly CompleteScreenerPageViewModel _viewModel;
        public CompleteScreenerPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = App.Container.Resolve<CompleteScreenerPageViewModel>();
        }
    }
}
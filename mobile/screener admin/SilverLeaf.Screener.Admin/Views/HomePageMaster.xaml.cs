using Autofac;
using SilverLeaf.Screener.Admin.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SilverLeaf.Screener.Admin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePageMaster : ContentPage
    {
        public HomePageMasterViewModel _viewModel;
        public ListView ListView => ListViewMenuItems;

        public HomePageMaster()
        {
            InitializeComponent();
            BindingContext = _viewModel = App.Container.Resolve<HomePageMasterViewModel>();
        }
    }
}
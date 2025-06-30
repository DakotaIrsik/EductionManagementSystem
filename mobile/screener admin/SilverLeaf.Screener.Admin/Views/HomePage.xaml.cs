using Autofac;
using SilverLeaf.Screener.Admin.Services.Interfaces;
using SilverLeaf.Screener.Admin.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SilverLeaf.Screener.Admin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : MasterDetailPage
    {
        private readonly HomePageViewModel _viewModel;

        public HomePage()
        {
            InitializeComponent();

            _viewModel = App.Container.Resolve<HomePageViewModel>();
            BindingContext = _viewModel;

            var masterViewModel = App.Container.Resolve<HomePageMasterViewModel>(TypedParameter.From(_viewModel));
            MasterPage.BindingContext = masterViewModel;

            _viewModel.PropertyChanged -= OnViewModelPropertyChanged;
            _viewModel.PropertyChanged += OnViewModelPropertyChanged;

            _viewModel.CurrentPage = HomePageViewModel.GetPage(HomePageViewModel.DesiredPage);

            MasterPage.ListView.ItemSelected -= OnDrawerItemSelected;
            MasterPage.ListView.ItemSelected += OnDrawerItemSelected;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == nameof(HomePageViewModel.CurrentPage))
            {
                Detail = new NavigationPage(_viewModel.CurrentPage);
                //Need to set the new navigation.
                App.Container.Resolve<INavigator>().Nav = Detail.Navigation;
            }
        }

        private void OnDrawerItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var list = (ListView)sender;
            list.SelectedItem = null; //Need to set it to null to remove the ugly looking background color.
            IsPresented = false; //Closes the drawer.
        }
    }
}
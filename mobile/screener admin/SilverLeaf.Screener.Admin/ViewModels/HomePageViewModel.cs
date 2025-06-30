using SilverLeaf.Screener.Admin.Services.Interfaces;
using SilverLeaf.Screener.Admin.Views;
using System;
using System.Reflection;
using Xamarin.Forms;

namespace SilverLeaf.Screener.Admin.ViewModels
{
    public class HomePageViewModel : BaseViewModel
    {
        private static string _desiredPage;
        private readonly INavigator _navigator;
        private readonly IUserService _userService;
        private Page _currentPage;

        public HomePageViewModel(
            INavigator navigator,
            IUserService userService,
            IProcessing processing,
            IAlerting alerting)
        {
            Track(nameof(HomePageViewModel));
            _navigator = navigator;
            _userService = userService;

            DesiredPageChanged -= SetCurrentPage;
            DesiredPageChanged += SetCurrentPage;
        }

        // Events
        public static event EventHandler DesiredPageChanged = delegate { };

        public static string DesiredPage
        {
            get => _desiredPage;
            set
            {
                _desiredPage = value;
                DesiredPageChanged(null, EventArgs.Empty);
            }
        }

        public Page CurrentPage
        {
            get => _currentPage;
            set => SetProperty(ref _currentPage, value);
        }

        public static Page GetPage(string pageName)
        {
            var assembly = typeof(HomePage).GetTypeInfo().Assembly;
            var pageType = assembly.GetType($"Screener.Views.{pageName}") ?? typeof(PendingScreenersPage);
            return Activator.CreateInstance(pageType) as Page;
        }

        public void OnAppearing()
        {
            Track(nameof(HomePageViewModel), nameof(OnAppearing));
            //var response = await _processing.Process(Task.WhenAll(_userService.RefreshUser())));
            //if (!response.Succeed)
            //{
            //    await _alerting.ShowDefaultWarning(response.GetFormattedErrors()); 
            //}

            if (_userService.User == null)
            {
                _navigator.ShowLogin();
            }
            //else
            //{
            //    await _navigator.PushModalAsync(new HomePage());
            //}
        }

        private void SetCurrentPage(object sender, EventArgs args)
        {
            Track(nameof(HomePageViewModel), nameof(SetCurrentPage));
            CurrentPage = GetPage(DesiredPage);
        }
    }
}

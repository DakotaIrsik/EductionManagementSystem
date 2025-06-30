using Microsoft.AppCenter.Analytics;
using SilverLeaf.Screener.Admin.Localization;
using SilverLeaf.Screener.Admin.Objects;
using SilverLeaf.Screener.Admin.Services.Interfaces;
using SilverLeaf.Screener.Admin.Views;
using System;
using Xamarin.Forms;

namespace SilverLeaf.Screener.Admin.ViewModels
{
    public class HomePageMasterViewModel : BaseViewModel
    {
        private readonly IUserService _userService;
        private readonly INavigator _navigator;
        private MenuItem _selectedMenuItem;

        public HomePageMasterViewModel(
            HomePageViewModel parent,
            IUserService userService,
            INavigator navigator)
        {
            Track(nameof(HomePageMasterViewModel));
            _userService = userService;
            _navigator = navigator;

            Parent = parent;
            GenerateMenu();

            LoginPageViewModel.LoginSuccessful -= SetSuccessfulLoginPage;
            LoginPageViewModel.LoginSuccessful += SetSuccessfulLoginPage;
        }

        public HomePageViewModel Parent { get; }
        public User User => _userService.User;

        public MenuItem SelectedMenuItem
        {
            get => _selectedMenuItem;
            set
            {
                if (SetProperty(ref _selectedMenuItem, value) && _selectedMenuItem != null)
                {
                    OnSelectedMenuItemChanged(_selectedMenuItem);
                }
            }
        }

        private void OnSelectedMenuItemChanged(MenuItem menuItem)
        {
            Track(nameof(HomePageMasterViewModel), nameof(OnSelectedMenuItemChanged));
            if (menuItem.Key == "MenuItemScreenerRegistration")
            {
                Parent.CurrentPage = new ScreenerRegistrationPage();
            }
            else if (menuItem.Key == "MenuItemPendingScreeners")
            {
                Parent.CurrentPage = new PendingScreenersPage();
            }
            else if (menuItem.Key == "MenuItemSettings")
            {
                Parent.CurrentPage = new SettingsPage();
            }
            else if (menuItem.Key == "MenuItemCompletedScreener")
            {
                Parent.CurrentPage = new CompletedScreenersPage();
            }

            else if (menuItem.Key == "MenuItemLogout")
            {
                _userService.ClearUserCache();
                _navigator.ShowLogin();
            }

        }

        private void SetSuccessfulLoginPage(object sender, EventArgs user)
        {
            Track(nameof(HomePageMasterViewModel), nameof(SetSuccessfulLoginPage));
            Parent.CurrentPage = new PendingScreenersPage();
        }
    }

    public class MenuItem : BaseViewModel
    {
        public MenuItem(string key)
        {
            MessagingCenter.Subscribe<object, CultureChangedMessage>(this,
             string.Empty, OnCultureChanged);
            Key = key;
        }

        private void OnCultureChanged(object s, CultureChangedMessage ccm)
        {
            Track(nameof(HomePageMasterViewModel), nameof(OnCultureChanged));
            OnPropertyChanged(nameof(MenuText));
        }
        private string _menuText;

        public string ImageResourceLocation { get; set; }
        public string MenuText
        {
            get { return Resources[Key]; }
            set { _menuText = value; OnPropertyChanged(nameof(MenuText)); }
        }
        public string Key { get; }

    }
}

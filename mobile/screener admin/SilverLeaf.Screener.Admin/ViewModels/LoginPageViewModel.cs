using Plugin.Connectivity;
using SilverLeaf.Screener.Admin.Objects;
using SilverLeaf.Screener.Admin.Resources;
using SilverLeaf.Screener.Admin.Services;
using SilverLeaf.Screener.Admin.Services.Interfaces;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SilverLeaf.Screener.Admin.ViewModels
{
    public class LoginPageViewModel : BaseViewModel
    {
        private readonly IUserService _userService;
        private readonly INavigator _navigator;
        private readonly IProcessing _processing;
        private readonly IAlerting _alerting;

        private string _username;
        private string _password = string.Empty;

        public LoginPageViewModel(IUserService userService,
                                  INavigator navigator,
                                  IProcessing processing,
                                  IAlerting alerting)
        {
            Track(nameof(LoginPageViewModel));
            _userService = userService;
            _navigator = navigator;
            _processing = processing;
            _alerting = alerting;

            SignInCommand = new Command(SignIn);
        }

        //// Events
        public static event EventHandler LoginSuccessful = delegate { };

        public string Username
        {
            get => _username ?? (_username = CacheService.LoginUserName);
            set => SetProperty(ref _username, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }
        public ICommand SignInCommand { get; }


        public async void OnAppearing()
        {
            Track(nameof(LoginPageViewModel), nameof(OnAppearing));
            if (!CrossConnectivity.Current.IsConnected)
            {
                await _alerting.ShowDefaultWarning(Lang.NetworkError);
            }
        }

        public async void SignIn()
        {
            Track(nameof(LoginPageViewModel), nameof(SignIn));
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password) || Username.Length < 3 || Password.Length < 6)
            {
                await _alerting.Show(Lang.LoginError, Lang.UsernamePasswordError, Lang.OK);
                return;
            }

            if (!CrossConnectivity.Current.IsConnected)
            {
                await _alerting.ShowDefaultWarning(Lang.NetworkError);
            }
            else
            {
                IsBusy = true;
                var response = await _processing.Process(_userService.Authenticate(Username, Password));
                IsBusy = false;
                if (response.Canceled)
                {
                    return;
                }

                if (!response.Succeed)
                {
                    await _alerting.ShowDefaultWarning(response.GetFormattedErrors());
                    return;
                }
                await SuccessfulLogin();
            }
        }

        private async Task SuccessfulLogin()
        {
            Track(nameof(LoginPageViewModel), nameof(SuccessfulLogin));
            LoginSuccessful(null, EventArgs.Empty);
            CacheService.User = new User()
            {
                UserName = _username
            };
            OnPropertyChanged(nameof(CacheService.LoginUserName));
            IsBusy = false;
            await _navigator.PopModalStackAsync();
        }
    }
}

using SilverLeaf.Screener.Admin.Localization;
using SilverLeaf.Screener.Admin.Services;
using SilverLeaf.Screener.Admin.Services.Interfaces;
using System.Collections.Generic;
using Xamarin.Forms;

namespace SilverLeaf.Screener.Admin.ViewModels
{
    public class SettingsPageViewModel : BaseViewModel
    {
        public List<string> Languages => new List<string> { "EN", "ZH" };


        private string _SelectedLanguage;
        private readonly INavigator _navigator;

        public string SelectedLanguage
        {
            get { return _SelectedLanguage; }
            set
            {
                _SelectedLanguage = value;
                SetLanguage();
            }
        }

        public SettingsPageViewModel(INavigator navigator)
        {
            _SelectedLanguage = CacheService.CurrentLanguage;
            _navigator = navigator;
        }

        private void SetLanguage()
        {
            CacheService.CurrentLanguage = SelectedLanguage;
            MessagingCenter.Send<object, CultureChangedMessage>(this,
                    string.Empty, new CultureChangedMessage(SelectedLanguage));
        }
    }
}
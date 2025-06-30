using Acr.UserDialogs;
using Microsoft.AppCenter.Analytics;
using Plugin.DownloadManager;
using Plugin.DownloadManager.Abstractions;
using SilverLeaf.Entities.DTOs;
using SilverLeaf.Screener.Admin.Localization;
using SilverLeaf.Screener.Admin.Resources;
using SilverLeaf.Screener.Admin.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SilverLeaf.Screener.Admin.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<MenuItem> MenuItems { get; set; }


        protected StudentDTO Student { get; set; }

        bool isDownloading = true;

        public ImageSource BackgroundImage => ImageSource.FromResource("SilverLeaf.Screener.Resources.Images.backgroundimage.png");

        public BaseViewModel()
        {
            Resources = new LocalizedResources(typeof(Lang), CacheService.CurrentLanguage);
            Student = CacheService.Student;
        }

        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        public LocalizedResources Resources
        {
            get;
            private set;
        }


        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName]string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        public void Track(string className, string methodName = "Constructor", string extraInformation = null, object extraData = null)
            {
            StringBuilder analyticsEvent = new StringBuilder();
            analyticsEvent.Append($"Class: {className} - Method: {methodName}");

            var analyticsEventDictionary = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(extraInformation))
            {
                analyticsEventDictionary.Add("Extra Information: ", extraInformation);
            }

            if (extraData != null)
            {
                var properties = extraData.GetType().GetProperties();
                foreach (var prop in properties)
                {
                    analyticsEventDictionary.Add(prop.Name, prop.GetValue(extraData).ToString());
                }
            }

            Analytics.TrackEvent($"Class: {className} - Method: {methodName}", analyticsEventDictionary);
            }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        #region Downloads
        protected bool IsDownloading(IDownloadFile file)
        {
            if (file == null)
            {
                return false;
            }

            switch (file.Status)
            {
                case DownloadFileStatus.INITIALIZED:
                case DownloadFileStatus.PAUSED:
                case DownloadFileStatus.PENDING:
                case DownloadFileStatus.RUNNING:
                    return true;
                case DownloadFileStatus.COMPLETED:
                case DownloadFileStatus.CANCELED:
                case DownloadFileStatus.FAILED:
                    return false;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


        public async void DownloadFile(string fileUrl)
        {
            await Task.Yield();
            await Task.Run(() =>
            {

                var downloadManager = CrossDownloadManager.Current;
                var file = downloadManager.CreateDownloadFile(fileUrl);
                downloadManager.Start(file, true);

                while (isDownloading)
                {
                    isDownloading = IsDownloading(file);
                }

            });

            if (!isDownloading)
            {
                UserDialogs.Instance.Toast(Lang.DownloadComplete);
            }
        }

        #endregion

        protected void GenerateMenu()
        {
            Track(nameof(BaseViewModel), nameof(GenerateMenu));
            MenuItems = new ObservableCollection<MenuItem>()
            {
                new MenuItem("MenuItemPendingScreeners") { ImageResourceLocation = "baseline_send_black.png", MenuText = Resources["MenuItemPendingScreeners"] },
                new MenuItem("MenuItemCompletedScreener") { ImageResourceLocation = "baseline_done_black.png", MenuText =  Resources["MenuItemCompletedScreener"] },
                new MenuItem("MenuItemScreenerRegistration") { ImageResourceLocation = "baseline_playlist_add_black.png", MenuText = Resources["MenuItemScreenerRegistration"] },
                new MenuItem("MenuItemSettings") { ImageResourceLocation = "baseline_settings_applications_black.png", MenuText =  Resources["MenuItemSettings"] },
                new MenuItem("MenuItemLogout") { ImageResourceLocation = "baseline_swap_horiz_black.png", MenuText =  Resources["MenuItemLogout"] }
            };
        }
        #endregion
    }
}

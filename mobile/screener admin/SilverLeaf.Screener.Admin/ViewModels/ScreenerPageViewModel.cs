using SilverLeaf.Entities.ViewModels.Requests.Responses;
using SilverLeaf.Screener.Admin.Services.Interfaces;
using System.Windows.Input;
using Xamarin.Forms;

namespace SilverLeaf.Screener.Admin.ViewModels
{
    public class ScreenerPageViewModel : BaseViewModel
    {
        private IProcessing _processing;
        public ScreenerSummaryResponse ScreenerSummary { get; set; }

        public ICommand DownloadCommand => new Command(Download);

        public ScreenerPageViewModel(IProcessing processing)
        {
            _processing = processing;
        }

        public void Download()
        {
            DownloadFile(ScreenerSummary.Url);
        }

        public void OnAppearing()
        {
            OnPropertyChanged(nameof(ScreenerSummary));
        }
    }
}
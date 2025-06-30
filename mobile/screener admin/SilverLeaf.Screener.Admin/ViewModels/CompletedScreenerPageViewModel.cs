using SilverLeaf.Common.Models;
using SilverLeaf.Entities.ViewModels.Requests.Responses;
using SilverLeaf.Screener.Admin.Services.Interfaces;
using SilverLeaf.Screener.Admin.Views;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SilverLeaf.Screener.Admin.ViewModels
{
    public class CompletedScreenerPageViewModel : BaseViewModel
    {
        private readonly ICompletionScreenerService _completionScreenerService;
        private readonly IProcessing _processing;
        private readonly INavigator _navigator;

        private ScreenerSummaryResponse selected;
        public ScreenerSummaryResponse Selected
        {
            get
            {
                return selected;
            }
            set
            {
                selected = value;
                RefreshPage();
            }
        }

        public AdjustableDTO<ScreenerSummaryResponse> Results => _completionScreenerService.CompletedScreeners ?? new AdjustableDTO<ScreenerSummaryResponse>();

        public bool HasNoResults => _completionScreenerService?.CompletedScreeners?.Data?.Count() == null ||
                                    _completionScreenerService?.CompletedScreeners?.Data?.Count() == 0;

        public bool HasResults => !HasNoResults;

        public bool IsNotSelected => Selected == null;

        public bool IsSelected => !IsNotSelected;

        public bool ShouldSelect => HasResults && IsNotSelected;

        public ICommand ViewScreenerReport => new Command(ViewScreener);

        public CompletedScreenerPageViewModel(ICompletionScreenerService completionScreenerService, IProcessing processing, INavigator navigator)
        {
            _completionScreenerService = completionScreenerService;
            _processing = processing;
            _navigator = navigator;

            _completionScreenerService.CompletedScreenersChanged -= HandleCustomEvent;
            _completionScreenerService.CompletedScreenersChanged += HandleCustomEvent;
        }

        public async Task OnAppearing()
        {
            await _processing.Process(_completionScreenerService.Load(), "Loading screeners").ConfigureAwait(false);
            RefreshPage();
        }

        void HandleCustomEvent(object sender, AdjustableDTO<ScreenerSummaryResponse> a)
        {
            RefreshPage();
        }

        private async void ViewScreener()
        {
            await _navigator.PushAsync(new ScreenerPage(Selected));
        }

        private void RefreshPage()
        {
            OnPropertyChanged(nameof(Selected));
            OnPropertyChanged(nameof(Results));
            OnPropertyChanged(nameof(HasResults));
            OnPropertyChanged(nameof(HasNoResults));
            OnPropertyChanged(nameof(IsNotSelected));
            OnPropertyChanged(nameof(IsSelected));
            OnPropertyChanged(nameof(ShouldSelect));
        }
    }
}
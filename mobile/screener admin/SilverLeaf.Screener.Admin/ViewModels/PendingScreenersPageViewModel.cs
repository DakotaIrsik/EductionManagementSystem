using SilverLeaf.Common.Models;
using SilverLeaf.Entities.DTOs;
using SilverLeaf.Screener.Admin.Services;
using SilverLeaf.Screener.Admin.Services.Interfaces;
using SilverLeaf.Screener.Admin.Views;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SilverLeaf.Screener.Admin.ViewModels
{
    public class PendingScreenersPageViewModel : BaseViewModel
    {
        private readonly INavigator _navigator;
        private readonly IPendingScreenerService _pendingScreenerService;
        private readonly IProcessing _processing;

        public AdjustableDTO<StudentDTO> Results => _pendingScreenerService.PendingStudentScreeners ?? new AdjustableDTO<StudentDTO>();

        private StudentDTO selected;
        public StudentDTO Selected
        {
            get
            {
                return selected;
            }
            set
            {
                selected = value;
                CacheService.Student = value;
                RefreshPage();
            }
        }

        public bool IsOralScreenerIncomplete => !IsNotSelected && ((Selected?.OralScreenerCompletionPercentage < 100) ? true : false);
        public bool IsPhonicsScreenerIncomplete => !IsNotSelected && ((Selected?.PhonicsScreenerCompletionPercentage < 100) ? true : false);
        public bool IsComprehensionScreenerIncomplete => !IsNotSelected && ((Selected?.ComprehensionScreenerCompletionPercentage < 100) ? true : false);
        public bool IsScreenerComplete => (Selected != null) &&
                                            (!IsOralScreenerIncomplete &&
                                            !IsPhonicsScreenerIncomplete &&
                                            !IsComprehensionScreenerIncomplete);

        public bool IsNotSelected => Selected == null;

        public bool IsSelected => !IsNotSelected;

        public bool HasNoResults => _pendingScreenerService?.PendingStudentScreeners?.Data?.Count() == null ||
                                    _pendingScreenerService?.PendingStudentScreeners?.Data?.Count() == 0;

        public bool HasResults => !HasNoResults;

        public bool ShouldSelect => HasResults && IsNotSelected;

        public ICommand ResumeOralScreenerCommand => new Command(ResumeOralScreener);
        public ICommand ResumeComprehensionScreenerCommand => new Command(ResumeComprehensionScreener);
        public ICommand ResumePhonicsScreenerCommand => new Command(ResumePhonicsScreener);
        public ICommand FinishScreenerCommand => new Command(SubmitScreener);

        public PendingScreenersPageViewModel(INavigator navigator, IPendingScreenerService pendingScreenerService, IProcessing processing)
        {
            Track(nameof(HomePageMasterViewModel));
            _navigator = navigator;
            _pendingScreenerService = pendingScreenerService;
            _processing = processing;
            _pendingScreenerService.PendingStudentScreenersChanged -= HandleCustomEvent;
            _pendingScreenerService.PendingStudentScreenersChanged += HandleCustomEvent;
            RefreshPage();
        }

        void HandleCustomEvent(object sender, AdjustableDTO<StudentDTO> a)
        {
            Track(nameof(HomePageMasterViewModel), nameof(HandleCustomEvent));
            RefreshPage();
        }

        public async Task OnAppearing()
        {
            Track(nameof(HomePageMasterViewModel), nameof(OnAppearing));
            await _processing.Process(_pendingScreenerService.Load(), "Loading screeners").ConfigureAwait(false);
            Selected = null;
            RefreshPage();
        }

        private async void ResumeOralScreener()
        {
            Track(nameof(HomePageMasterViewModel), nameof(ResumeOralScreener));
            await _navigator.PushAsync(new OralScreenerQuestionPage());
        }
        private async void ResumeComprehensionScreener()
        {
            Track(nameof(HomePageMasterViewModel), nameof(ResumeComprehensionScreener));
            await _navigator.PushAsync(new ComprehensionScreenerQuestionPage());
        }
        private async void ResumePhonicsScreener()
        {
            Track(nameof(HomePageMasterViewModel), nameof(ResumePhonicsScreener));
            await _navigator.PushAsync(new PhonicsScreenerQuestionPage());
        }
        private async void SubmitScreener()
        {
            Track(nameof(HomePageMasterViewModel), nameof(SubmitScreener));
            await _navigator.PushAsync(new CompleteScreenerPage());
        }

        private void RefreshPage()
        {
            OnPropertyChanged(nameof(Selected));
            OnPropertyChanged(nameof(Results));
            OnPropertyChanged(nameof(IsOralScreenerIncomplete));
            OnPropertyChanged(nameof(IsPhonicsScreenerIncomplete));
            OnPropertyChanged(nameof(IsComprehensionScreenerIncomplete));
            OnPropertyChanged(nameof(IsScreenerComplete));
            OnPropertyChanged(nameof(IsNotSelected));
            OnPropertyChanged(nameof(HasNoResults));
            OnPropertyChanged(nameof(HasResults));
            OnPropertyChanged(nameof(ShouldSelect));
        }
    }
}
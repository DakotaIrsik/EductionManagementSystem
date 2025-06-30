using SilverLeaf.Entities.ViewModels.Requests;
using SilverLeaf.Screener.Admin.Services.Interfaces;
using SilverLeaf.Screener.Admin.Views;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace SilverLeaf.Screener.Admin.ViewModels
{
    public class CompleteScreenerPageViewModel : BaseViewModel
    {
        private readonly ICompletionScreenerService _completionScreenerService;
        private readonly INavigator _navigator;
        public ICommand GetCourseRecommendationCommand => new Command(GetCourseRecommendation);

        public CompleteScreenerRequest CompleteScreenerRequest { get; set; } = new CompleteScreenerRequest();

        public CompleteScreenerPageViewModel(INavigator navigator, ICompletionScreenerService completionScreenerService, IProcessing processing)
        {
            _completionScreenerService = completionScreenerService;
            _navigator = navigator;
        }

        private async void GetCourseRecommendation()
        {
            await _completionScreenerService.SubmitAsync(CompleteScreenerRequest);
            _navigator.InsertPageBefore(new CompletedScreenersPage(), _navigator.NavigationStack.FirstOrDefault());
            await _navigator.PopToRootAsync();
        }
    }
}
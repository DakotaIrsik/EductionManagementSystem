using SilverLeaf.Screener.Admin.Models;
using SilverLeaf.Screener.Admin.Services;
using SilverLeaf.Screener.Admin.Services.Interfaces;
using SilverLeaf.Screener.Admin.Views.Renderers;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SilverLeaf.Screener.Admin.ViewModels
{
    public class PhonicsScreenerQuestionPageViewModel : BaseViewModel
    {
        private readonly INavigator _navigator;
        private readonly IPhonicsScreenerService _phonicsScreenerService;
        private readonly IProcessing _processing;

        public PhonicsScreenerViewModel Question => _phonicsScreenerService.NextQuestion;
        public bool IsFinished => _phonicsScreenerService.NextQuestion == null;
        public bool IsNotFinished => !IsFinished;
        public IRenderer Renderer { get; set; }
        public ICommand SuccessCommand => new Command(Success);
        public ICommand CautionCommand => new Command(Caution);
        public ICommand ResetCommand => new Command(Reset);
        public ICommand SaveCommand => new Command(Save);
        public ICommand CannotCompleteCommand => new Command(CannotComplete);

        public PhonicsScreenerQuestionPageViewModel(INavigator navigator, IPhonicsScreenerService screenerService, IProcessing processing)
        {
            _phonicsScreenerService = screenerService;
            _navigator = navigator;
            _processing = processing;
            _phonicsScreenerService.NextQuestionChanged += NextQuestionChanged;
            Renderer = new ProgressRenderer();
        }

        public async Task OnAppearing()
        {
            await _processing.Process(_phonicsScreenerService.LoadNextQuestion());
        }

        void NextQuestionChanged(object sender, EventArgs e)
        {
            RefreshPage();
        }

        public async void Save()
        {
            await _processing.Process(_phonicsScreenerService.SubmitAnswers());
            await _navigator.PopAsync();
        }

        public void Reset()
        {
            CacheService.IncompletePhonicsScreenerQuestions = null;
            _phonicsScreenerService.LoadNextQuestion();
            RefreshPage();
        }

        public void Success()
        {
            Question.IsCorrect = true;
            SaveInCacheThenReloadOrQuit();
        }

        public void Caution()
        {
            Question.IsCorrect = false;
            SaveInCacheThenReloadOrQuit();
        }

        public async void CannotComplete()
        {
            await _phonicsScreenerService.CautionAllUnanswered();
            RefreshPage();
        }

        public void RefreshPage()
        {
            OnPropertyChanged(nameof(Question));
            OnPropertyChanged(nameof(IsNotFinished));
            OnPropertyChanged(nameof(IsFinished));
            Renderer.Refresh(CacheService.Student.PhonicsScreenerCompletionPercentage,
                                     _phonicsScreenerService.RecentlyCompleted,
                                     _phonicsScreenerService.ToBeCompleted);
        }

        private void SaveInCacheThenReloadOrQuit()
        {
            _phonicsScreenerService.AnswerQuestion(Question);
            _phonicsScreenerService.LoadNextQuestion();
            RefreshPage();
        }
    }
}
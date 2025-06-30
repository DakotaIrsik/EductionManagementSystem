using SilverLeaf.Screener.Admin.Models;
using SilverLeaf.Screener.Admin.Services;
using SilverLeaf.Screener.Admin.Services.Interfaces;
using SilverLeaf.Screener.Admin.Views.Renderers;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace SilverLeaf.Screener.Admin.ViewModels
{
    public class OralScreenerQuestionPageViewModel : BaseViewModel
    {
        private readonly INavigator _navigator;
        private readonly IOralScreenerService _oralScreenerService;
        private readonly IProcessing _processing;

        public OralScreenerViewModel Question => _oralScreenerService.NextQuestion;
        public bool IsFinished => _oralScreenerService.NextQuestion == null;
        public bool IsNotFinished => !IsFinished;
        public IRenderer Renderer { get; set; }
        public ICommand SuccessCommand => new Command(Success);
        public ICommand CautionCommand => new Command(Caution);
        public ICommand ResetCommand => new Command(Reset);
        public ICommand SaveCommand => new Command(Save);
        public ICommand CannotCompleteCommand => new Command(CannotComplete);

        public OralScreenerQuestionPageViewModel(INavigator navigator, IOralScreenerService screenerService, IProcessing processing)
        {
            _oralScreenerService = screenerService;
            _navigator = navigator;
            _processing = processing;
            _oralScreenerService.NextQuestionChanged += NextQuestionChanged;
            Renderer = new ProgressRenderer();
        }

        public async void OnAppearing()
        {
            await _processing.Process(_oralScreenerService.LoadNextQuestion());
        }

        void NextQuestionChanged(object sender, EventArgs e)
        {
            RefreshPage();
        }

        public async void Save()
        {
            await _processing.Process(_oralScreenerService.SubmitAnswers());
            await _navigator.PopAsync();
        }

        public void Reset()
        {
            CacheService.IncompleteOralScreenerQuestions = null;
            _oralScreenerService.LoadNextQuestion();
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
            await _oralScreenerService.CautionAllUnanswered();
            RefreshPage();
        }

        public void RefreshPage()
        {
            OnPropertyChanged(nameof(Question));
            OnPropertyChanged(nameof(IsNotFinished));
            OnPropertyChanged(nameof(IsFinished));
            Renderer.Refresh(CacheService.Student.OralScreenerCompletionPercentage,
                                     _oralScreenerService.RecentlyCompleted,
                                     _oralScreenerService.ToBeCompleted);
        }

        private void SaveInCacheThenReloadOrQuit()
        {
            _oralScreenerService.AnswerQuestion(Question);
            _oralScreenerService.LoadNextQuestion();
            RefreshPage();
        }
    }
}
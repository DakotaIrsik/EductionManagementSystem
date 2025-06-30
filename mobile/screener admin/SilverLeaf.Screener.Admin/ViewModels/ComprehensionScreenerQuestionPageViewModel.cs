using SilverLeaf.Screener.Admin.Models;
using SilverLeaf.Screener.Admin.Services;
using SilverLeaf.Screener.Admin.Services.Interfaces;
using SilverLeaf.Screener.Admin.Views.Renderers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SilverLeaf.Screener.Admin.ViewModels
{
    public class ComprehensionScreenerQuestionPageViewModel : BaseViewModel
    {
        private readonly INavigator _navigator;
        private readonly IComprehensionScreenerService _comprehensionScreenerService;
        private readonly IProcessing _processing;

        public ComprehensionScreenerViewModel Question => _comprehensionScreenerService.NextQuestion;
        public bool IsFinished => _comprehensionScreenerService.NextQuestion == null;
        public bool IsNotFinished => !IsFinished;
        public IRenderer Renderer { get; set; }
        public IEnumerable<ComprehensionScreenerSectionViewModel> QuestionSections => new List<ComprehensionScreenerSectionViewModel>()
        {
            new ComprehensionScreenerSectionViewModel(Question?.LocalImage, Question?.Preface),
            new ComprehensionScreenerSectionViewModel(Question?.SecondLocalImage, Question?.SecondPreface),
        };
        public ICommand SuccessCommand => new Command(Success);
        public ICommand CautionCommand => new Command(Caution);
        public ICommand ResetCommand => new Command(Reset);
        public ICommand SaveCommand => new Command(Save);
        public ICommand CannotCompleteCommand => new Command(CannotComplete);

        public ComprehensionScreenerQuestionPageViewModel(INavigator navigator, IComprehensionScreenerService screenerService, IProcessing processing)
        {
            _comprehensionScreenerService = screenerService;
            _navigator = navigator;
            _processing = processing;
            _comprehensionScreenerService.NextQuestionChanged += NextQuestionChanged;
            Renderer = new ProgressRenderer();
        }

        public async Task OnAppearing()
        {
            await _processing.Process(_comprehensionScreenerService.LoadNextQuestion());
        }

        void NextQuestionChanged(object sender, EventArgs e)
        {
            RefreshPage();
        }

        public async void Save()
        {
            await _processing.Process(_comprehensionScreenerService.SubmitAnswers());
            await _navigator.PopAsync();
        }

        public void Reset()
        {
            CacheService.IncompleteComprehensionScreenerQuestions = null;
            _comprehensionScreenerService.LoadNextQuestion();
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
            await _comprehensionScreenerService.CautionAllUnanswered();
            RefreshPage();
        }

        private void RefreshPage()
        {
            OnPropertyChanged(nameof(Question));
            OnPropertyChanged(nameof(QuestionSections));
            OnPropertyChanged(nameof(IsNotFinished));
            OnPropertyChanged(nameof(IsFinished));
            Renderer.Refresh(CacheService.Student.OralScreenerCompletionPercentage,
                                     _comprehensionScreenerService.RecentlyCompleted,
                                     _comprehensionScreenerService.ToBeCompleted);
        }

        private void SaveInCacheThenReloadOrQuit()
        {
            _comprehensionScreenerService.AnswerQuestion(Question);
            _comprehensionScreenerService.LoadNextQuestion();
            RefreshPage();
        }
    }
}
using SilverLeaf.Entities.DTOs;
using SilverLeaf.Screener.Admin.Services.Interfaces;
using SilverLeaf.Screener.Admin.Views;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace SilverLeaf.Screener.Admin.ViewModels
{
    public class ScreenerRegistrationPageViewModel : BaseViewModel
    {

        private readonly IProcessing _processing;
        private readonly INavigator _navigator;
        private readonly IStudentService _studentService;
        public HomePageViewModel Parent { get; }

        private int _age;
        private string _grade;
        private string _englishName;
        private string _assessor;


        public ScreenerRegistrationPageViewModel(IProcessing processing, INavigator navigator, IStudentService studentService, HomePageViewModel parent)
        {
            _processing = processing;
            _navigator = navigator;
            _studentService = studentService;
            Parent = parent;
        }

        public bool CanSubmit => !string.IsNullOrEmpty(Age.ToString()) && !string.IsNullOrEmpty(EnglishName) && !string.IsNullOrEmpty(Assessor) && !string.IsNullOrEmpty(Grade);

        public Color SearchButtonBackgroundColor => CanSubmit ? (Color)Application.Current.Resources["Primary"] : (Color)Application.Current.Resources["DisabledButtonBackgroundColor"];

        public Color SearchButtonTextColor => CanSubmit ? (Color)Application.Current.Resources["White"] : (Color)Application.Current.Resources["LightTextColor"];

        public ICommand SubmitCommand => new Command(SaveNewScreener);

        public ICommand ResetCommand => new Command(Reset);

        public IEnumerable<int> Ages => new List<int> { 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };

        public IEnumerable<string> Grades => new List<string> { "K", 1.ToString(), 2.ToString(), 3.ToString(), 4.ToString(), 5.ToString(), 6.ToString(), 7.ToString(), 8.ToString(), 9.ToString(), 10.ToString(), 11.ToString(), 12.ToString() };

        public int Age
        {
            get => (_age == 0) ? 4 : _age;
            set
            {
                _age = value;
                UpdateSubmissionEligibility();

            }
        }

        public string EnglishName
        {
            get => _englishName;
            set
            {
                _englishName = value;
                UpdateSubmissionEligibility();

            }
        }

        public string Assessor
        {
            get => _assessor;
            set
            {
                _assessor = value;
                UpdateSubmissionEligibility();
            }
        }

        public string Grade
        {
            get => _grade;
            set
            {
                _grade = value;
                UpdateSubmissionEligibility();
            }
        }

        private async void SaveNewScreener()
        {
            await _processing.Process(_studentService.Register(new StudentDTO()
            {
                Age = Age,
                Assessor = Assessor,
                EnglishName = EnglishName,
                Grade = Grade
            }));

            _navigator.InsertPageBefore(new PendingScreenersPage(), _navigator.NavigationStack.FirstOrDefault());
            await _navigator.PopAsync();
        }

        private void UpdateSubmissionEligibility()
        {
            OnPropertyChanged(nameof(SearchButtonBackgroundColor));
            OnPropertyChanged(nameof(SearchButtonTextColor));
            OnPropertyChanged(nameof(CanSubmit));
        }


        private void Reset()
        {
            Age = 4;
            Grade = null;
            Assessor = null;
            EnglishName = null;
        }
    }
}

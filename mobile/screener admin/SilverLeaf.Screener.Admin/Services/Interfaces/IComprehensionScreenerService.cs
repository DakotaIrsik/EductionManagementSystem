using SilverLeaf.Screener.Admin.Models;
using System;
using System.Threading.Tasks;

namespace SilverLeaf.Screener.Admin.Services.Interfaces
{
    public interface IComprehensionScreenerService
    {
        Task LoadNextQuestion();

        void AnswerQuestion(ComprehensionScreenerViewModel result);

        Task SubmitAnswers();

        Task CautionAllUnanswered();

        ComprehensionScreenerViewModel NextQuestion { get; set; }

        event EventHandler NextQuestionChanged;

        bool HasUnsubmittedAnswers { get; }

        int ToBeCompleted { get; }

        int RecentlyCompleted { get; }
    }
}

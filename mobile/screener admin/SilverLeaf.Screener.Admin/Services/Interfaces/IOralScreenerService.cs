using SilverLeaf.Screener.Admin.Models;
using System;
using System.Threading.Tasks;

namespace SilverLeaf.Screener.Admin.Services.Interfaces
{
    public interface IOralScreenerService
    {
        Task LoadNextQuestion();

        void AnswerQuestion(OralScreenerViewModel result);

        Task SubmitAnswers();

        Task CautionAllUnanswered();

        OralScreenerViewModel NextQuestion { get; set; }

        event EventHandler NextQuestionChanged;

        bool HasUnsubmittedAnswers { get; }

        int ToBeCompleted { get; }

        int RecentlyCompleted { get; }
    }
}

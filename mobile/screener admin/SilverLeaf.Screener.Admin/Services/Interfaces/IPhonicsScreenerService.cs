using SilverLeaf.Screener.Admin.Models;
using System;
using System.Threading.Tasks;

namespace SilverLeaf.Screener.Admin.Services.Interfaces
{
    public interface IPhonicsScreenerService
    {
        Task LoadNextQuestion();

        void AnswerQuestion(PhonicsScreenerViewModel result);

        Task SubmitAnswers();

        Task CautionAllUnanswered();

        PhonicsScreenerViewModel NextQuestion { get; set; }

        event EventHandler NextQuestionChanged;

        bool HasUnsubmittedAnswers { get; }

        int ToBeCompleted { get; }

        int RecentlyCompleted { get; }
    }
}

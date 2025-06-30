using SilverLeaf.Common.Interfaces;
using SilverLeaf.Common.Models;
using SilverLeaf.Entities.ViewModels.Requests;
using SilverLeaf.Screener.Admin.Models;
using SilverLeaf.Screener.Admin.Services.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SilverLeaf.Screener.Admin.Services
{
    public class PhonicsScreenerService : IPhonicsScreenerService
    {
        private readonly IPhonicsScreenerAPI _phonicsScreenerApi;

        public PhonicsScreenerViewModel NextQuestion { get; set; }

        public int RecentlyCompleted => CacheService.IncompletePhonicsScreenerQuestions?.Where(ipsq => ipsq.IsCorrect != null).Count() ?? 0;

        public int ToBeCompleted => CacheService.IncompletePhonicsScreenerQuestions?.Count() ?? 0;

        public event EventHandler NextQuestionChanged;

        public PhonicsScreenerService(IPhonicsScreenerAPI phonicsScreenerApi)
        {
            _phonicsScreenerApi = phonicsScreenerApi;
            CacheService.IncompletePhonicsScreenerQuestions = null;
        }

        public async Task LoadNextQuestion()
        {
            if (CacheService.IncompletePhonicsScreenerQuestions == null ||
                CacheService.IncompletePhonicsScreenerQuestions.Count() == 0 ||
                CacheService.IncompletePhonicsScreenerQuestions.FirstOrDefault().StudentId != CacheService.Student.Id)
            {
                var response = await _phonicsScreenerApi.LoadIncomplete<PhonicsScreenerViewModel>(new StudentQuestionsRequest
                {
                    StudentId = CacheService.Student.Id,
                    PagingRequest = new PagingRequest(0, "+Order", "", 300),
                }).ConfigureAwait(false);
                CacheService.IncompletePhonicsScreenerQuestions = response.Content;
            }

            NextQuestion = CacheService.IncompletePhonicsScreenerQuestions.Where(psq => psq.IsCorrect == null).OrderBy(psq => psq.Order).Take(1).SingleOrDefault();
            NextQuestionChanged?.Invoke(this, new EventArgs());
        }

        public void AnswerQuestion(PhonicsScreenerViewModel model)
        {
            var cachedIncomplete = CacheService.IncompletePhonicsScreenerQuestions?.SingleOrDefault(psq => psq.Id == model.Id);
            if (cachedIncomplete != null)
            {
                cachedIncomplete.IsCorrect = model.IsCorrect;
            }
        }

        public bool HasUnsubmittedAnswers => CacheService.IncompletePhonicsScreenerQuestions?.Any(ipsq => ipsq.IsCorrect != null) ?? false;

        public async Task SubmitAnswers()
        {
            var copy = CacheService.IncompletePhonicsScreenerQuestions.Where(ipsq => ipsq.IsCorrect != null).ToList();
            CacheService.IncompletePhonicsScreenerQuestions.RemoveAll(ipsc => ipsc.IsCorrect != null);
            var response = await _phonicsScreenerApi.BulkAnswerQuestions<string>(copy).ConfigureAwait(false);

        }

        public async Task CautionAllUnanswered()
        {
            var unAnswered = CacheService.IncompletePhonicsScreenerQuestions.Where(iosq => iosq.IsCorrect == null);
            foreach (var screener in unAnswered)
            {
                screener.IsCorrect = false;
            }
            await LoadNextQuestion();
        }
    }
}

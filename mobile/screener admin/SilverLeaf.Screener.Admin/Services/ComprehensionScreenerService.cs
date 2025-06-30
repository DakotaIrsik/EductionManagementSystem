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
    public class ComprehensionScreenerService : IComprehensionScreenerService
    {
        private readonly IComprehensionScreenerAPI _comprehensionScreenerApi;

        public ComprehensionScreenerViewModel NextQuestion { get; set; }

        public int RecentlyCompleted => CacheService.IncompleteComprehensionScreenerQuestions?.Where(ipsq => ipsq.IsCorrect != null).Count() ?? 0;

        public int ToBeCompleted => CacheService.IncompleteComprehensionScreenerQuestions?.Count() ?? 0;

        public event EventHandler NextQuestionChanged;

        public ComprehensionScreenerService(IComprehensionScreenerAPI comprehensionScreenerApi)
        {
            _comprehensionScreenerApi = comprehensionScreenerApi;
            CacheService.IncompleteComprehensionScreenerQuestions = null;
        }

        public async Task LoadNextQuestion()
        {
            if (CacheService.IncompleteComprehensionScreenerQuestions == null ||
                CacheService.IncompleteComprehensionScreenerQuestions.Count() == 0 ||
                CacheService.IncompleteComprehensionScreenerQuestions.FirstOrDefault().StudentId != CacheService.Student.Id)
            {
                var response = await _comprehensionScreenerApi.LoadIncomplete<ComprehensionScreenerViewModel>(new StudentQuestionsRequest
                {
                    StudentId = CacheService.Student.Id,
                    PagingRequest = new PagingRequest(0, "+Order", "", 300),
                }).ConfigureAwait(false);
                CacheService.IncompleteComprehensionScreenerQuestions = response.Content;
            }

            NextQuestion = CacheService.IncompleteComprehensionScreenerQuestions.Where(psq => psq.IsCorrect == null).OrderBy(psq => psq.Order).Take(1).SingleOrDefault();
            NextQuestionChanged?.Invoke(this, new EventArgs());
        }

        public void AnswerQuestion(ComprehensionScreenerViewModel model)
        {
            var cachedIncomplete = CacheService.IncompleteComprehensionScreenerQuestions?.SingleOrDefault(psq => psq.Id == model.Id);
            if (cachedIncomplete != null)
            {
                cachedIncomplete.IsCorrect = model.IsCorrect;
            }
        }

        public bool HasUnsubmittedAnswers => CacheService.IncompleteComprehensionScreenerQuestions?.Any(ipsq => ipsq.IsCorrect != null) ?? false;

        public async Task SubmitAnswers()
        {
            var copy = CacheService.IncompleteComprehensionScreenerQuestions.Where(ipsq => ipsq.IsCorrect != null).ToList();
            CacheService.IncompleteComprehensionScreenerQuestions.RemoveAll(ipsc => ipsc.IsCorrect != null);
            await _comprehensionScreenerApi.BulkAnswerQuestions<string>(copy).ConfigureAwait(false);

        }

        public async Task CautionAllUnanswered()
        {
            var unAnswered = CacheService.IncompleteComprehensionScreenerQuestions.Where(iosq => iosq.IsCorrect == null);
            foreach (var screener in unAnswered)
            {
                screener.IsCorrect = false;
            }
            await LoadNextQuestion();
        }
    }
}

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
    public class OralScreenerService : IOralScreenerService
    {
        private readonly IOralScreenerAPI _oralScreenerApi;

        public OralScreenerViewModel NextQuestion { get; set; }

        public int RecentlyCompleted => CacheService.IncompleteOralScreenerQuestions?.Where(ipsq => ipsq.IsCorrect != null).Count() ?? 0;

        public int ToBeCompleted => CacheService.IncompleteOralScreenerQuestions?.Count() ?? 0;

        public bool HasUnsubmittedAnswers => CacheService.IncompleteOralScreenerQuestions?.Any(ipsq => ipsq.IsCorrect != null) ?? false;

        public event EventHandler NextQuestionChanged;

        public OralScreenerService(IOralScreenerAPI oralScreenerApi)
        {
            _oralScreenerApi = oralScreenerApi;
            CacheService.IncompleteOralScreenerQuestions = null;
        }

        public async Task LoadNextQuestion()
        {
            if (CacheService.IncompleteOralScreenerQuestions == null ||
                CacheService.IncompleteOralScreenerQuestions.Count() == 0 ||
                CacheService.IncompleteOralScreenerQuestions.FirstOrDefault().StudentId != CacheService.Student.Id)
            {
                var response = await _oralScreenerApi.LoadIncomplete<OralScreenerViewModel>(new StudentQuestionsRequest
                {
                    StudentId = CacheService.Student.Id,
                    PagingRequest = new PagingRequest(0, "+Order", "", 300),
                }).ConfigureAwait(false);
                CacheService.IncompleteOralScreenerQuestions = response.Content;
            }

            NextQuestion = CacheService.IncompleteOralScreenerQuestions.Where(psq => psq.IsCorrect == null).OrderBy(psq => psq.Order).Take(1).SingleOrDefault();
            NextQuestionChanged?.Invoke(this, new EventArgs());
        }

        public void AnswerQuestion(OralScreenerViewModel model)
        {
            var cachedIncomplete = CacheService.IncompleteOralScreenerQuestions?.SingleOrDefault(psq => psq.Id == model.Id);
            if (cachedIncomplete != null)
            {
                cachedIncomplete.IsCorrect = model.IsCorrect;
            }
        }

        public async Task SubmitAnswers()
        {
            var copy = CacheService.IncompleteOralScreenerQuestions.Where(ipsq => ipsq.IsCorrect != null).ToList();
            CacheService.IncompleteOralScreenerQuestions.RemoveAll(ipsc => ipsc.IsCorrect != null);
            await _oralScreenerApi.BulkAnswerQuestions<string>(copy).ConfigureAwait(false);

        }

        public async Task CautionAllUnanswered()
        {
            var unAnswered = CacheService.IncompleteOralScreenerQuestions.Where(iosq => iosq.IsCorrect == null);
            foreach (var screener in unAnswered)
            {
                screener.IsCorrect = false;
            }
            await LoadNextQuestion();
        }
    }
}

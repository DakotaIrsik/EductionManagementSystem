using SilverLeaf.Common.Interfaces;
using SilverLeaf.Common.Models;
using SilverLeaf.Entities.ViewModels.Requests;
using SilverLeaf.Entities.ViewModels.Requests.Responses;
using SilverLeaf.Screener.Admin.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace SilverLeaf.Screener.Admin.Services
{
    public class CompletionScreenerService : ICompletionScreenerService
    {
        private readonly ICompletionScreenerAPI _completionScreenerApi;

        public AdjustableDTO<ScreenerSummaryResponse> CompletedScreeners { get; set; }

        public event EventHandler<AdjustableDTO<ScreenerSummaryResponse>> CompletedScreenersChanged;

        public CompletionScreenerService(ICompletionScreenerAPI completionScreenerApi)
        {
            _completionScreenerApi = completionScreenerApi;
        }

        public async Task SubmitAsync(CompleteScreenerRequest model)
        {
            model.StudentId = CacheService.Student.Id;
            var response = await _completionScreenerApi.Submit(model).ConfigureAwait(false);
        }

        public async Task Load()
        {
            var response = await _completionScreenerApi.Load<ScreenerSummaryResponse>(new CompletedScreenerRequest(new PagingRequest()
            {
            })).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                CompletedScreeners = response.Content;
                CompletedScreenersChanged?.Invoke(this, response.Content);
            }

        }
    }
}

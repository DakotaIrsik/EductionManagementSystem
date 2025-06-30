using SilverLeaf.Common.Models;
using SilverLeaf.Entities.ViewModels.Requests;
using SilverLeaf.Entities.ViewModels.Requests.Responses;
using System;
using System.Threading.Tasks;

namespace SilverLeaf.Screener.Admin.Services.Interfaces
{
    public interface ICompletionScreenerService
    {
        Task SubmitAsync(CompleteScreenerRequest model);

        Task Load();

        AdjustableDTO<ScreenerSummaryResponse> CompletedScreeners { get; set; }

        event EventHandler<AdjustableDTO<ScreenerSummaryResponse>> CompletedScreenersChanged;
    }
}

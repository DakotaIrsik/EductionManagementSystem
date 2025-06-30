using SilverLeaf.Common.Interfaces;
using SilverLeaf.Common.Models;
using SilverLeaf.Entities.DTOs;
using SilverLeaf.Entities.ViewModels.Requests;
using SilverLeaf.Screener.Admin.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace SilverLeaf.Screener.Admin.Services
{
    public class PendingScreenerService : IPendingScreenerService
    {
        private readonly IPendingScreenerAPI _pendingScreenerApi;

        public AdjustableDTO<StudentDTO> PendingStudentScreeners { get; set; }

        public event EventHandler<AdjustableDTO<StudentDTO>> PendingStudentScreenersChanged;

        public PendingScreenerService(IPendingScreenerAPI pendingScreenerApi)
        {
            _pendingScreenerApi = pendingScreenerApi;

        }

        public async Task Load()
        {
            var response = await _pendingScreenerApi.PendingScreener<StudentDTO>(new StudentSearchRequest(new PagingRequest()
            {
                Sort = "-UpdateDate"
            })).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                PendingStudentScreeners = response.Content;
                PendingStudentScreenersChanged?.Invoke(this, response.Content);
            }
        }
    }
}

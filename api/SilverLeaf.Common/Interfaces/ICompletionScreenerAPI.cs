using Refit;
using SilverLeaf.Common.Models;
using System.Threading.Tasks;

namespace SilverLeaf.Common.Interfaces
{
    public interface ICompletionScreenerAPI
    {
        [Post("/api/v1/CompletionScreener")]
        Task<string> Submit([Body] object request);

        [Post("/api/v1/CompletionScreener/Get")]
        Task<ApiResponse<AdjustableDTO<T>>> Load<T>([Body] object request);
    }
}

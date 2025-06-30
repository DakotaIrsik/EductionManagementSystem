using Refit;
using SilverLeaf.Common.Models;
using System.Threading.Tasks;

namespace SilverLeaf.Common.Interfaces
{
    public interface IPendingScreenerAPI
    {
        [Post("/api/v1/PendingScreener")]
        Task<ApiResponse<AdjustableDTO<T>>> PendingScreener<T>([Body] object request);
    }
}

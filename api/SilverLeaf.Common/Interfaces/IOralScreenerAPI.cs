using Refit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SilverLeaf.Common.Interfaces
{
    public interface IOralScreenerAPI
    {
        [Post("/api/v1/OralScreener")]
        Task<ApiResponse<List<T>>> Load<T>([Body] object request);

        [Post("/api/v1/OralScreener/Incomplete")]
        Task<ApiResponse<List<T>>> LoadIncomplete<T>([Body] object request);

        [Put("/api/v1/OralScreener")]
        Task<ApiResponse<List<T>>> AnswerQuestion<T>([Body] object request);

        [Put("/api/v1/OralScreener/Bulk")]
        Task<ApiResponse<List<T>>> BulkAnswerQuestions<T>([Body] object request);
    }
}

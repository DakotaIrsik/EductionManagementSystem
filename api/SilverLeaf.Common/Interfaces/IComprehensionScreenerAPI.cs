using Refit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SilverLeaf.Common.Interfaces
{
    public interface IComprehensionScreenerAPI
    {
        [Post("/api/v1/ComprehensionScreener")]
        Task<ApiResponse<List<T>>> Load<T>([Body] object request);

        [Post("/api/v1/ComprehensionScreener/Incomplete")]
        Task<ApiResponse<List<T>>> LoadIncomplete<T>([Body] object request);

        [Put("/api/v1/ComprehensionScreener")]
        Task<ApiResponse<List<T>>> AnswerQuestion<T>([Body] object request);

        [Put("/api/v1/ComprehensionScreener/Bulk")]
        Task<ApiResponse<List<T>>> BulkAnswerQuestions<T>([Body] object request);
    }
}

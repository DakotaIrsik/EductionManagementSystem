using Refit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SilverLeaf.Common.Interfaces
{
    public interface IPhonicsScreenerAPI
    {
        [Post("/api/v1/PhonicsScreener")]
        Task<ApiResponse<List<T>>> Load<T>([Body] object request);

        [Post("/api/v1/PhonicsScreener/Incomplete")]
        Task<ApiResponse<List<T>>> LoadIncomplete<T>([Body] object request);

        [Put("/api/v1/PhonicsScreener")]
        Task<ApiResponse<List<T>>> AnswerQuestion<T>([Body] object request);

        [Put("/api/v1/PhonicsScreener/Bulk")]
        Task<ApiResponse<List<T>>> BulkAnswerQuestions<T>([Body] object request);
    }
}

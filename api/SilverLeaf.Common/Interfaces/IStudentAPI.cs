using Refit;
using System.Threading.Tasks;

namespace SilverLeaf.Common.Interfaces
{
    public interface IStudentAPI
    {
        [Post("/api/v1/Student/CreateStudent")]
        Task<ApiResponse<T>> Register<T>([Body] object request);
    }
}

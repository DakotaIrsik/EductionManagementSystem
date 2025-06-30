using Refit;
using System.Threading.Tasks;

namespace SilverLeaf.Common.Interfaces
{
    public interface IUserAPI
    {
        [Post("/api/v1/User")]
        Task<ApiResponse<T>> Login<T>([Body] object request);
    }
}

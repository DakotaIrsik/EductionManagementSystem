using Refit;
using SilverLeaf.Common.Models;
using System.Threading.Tasks;

namespace SilverLeaf.Common
{
    public interface IIdentityServerAPI
    {

        [Post("/api/Account")]
        Task<RegistrationResponse> Register();
    }
}

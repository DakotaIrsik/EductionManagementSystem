using SilverLeaf.Screener.Admin.Objects;
using System.Threading.Tasks;

namespace SilverLeaf.Screener.Admin.Services.Interfaces
{
    public interface IUserService
    {
        User User { get; }

        Task Authenticate(string username, string password);

        void ClearUserCache();

    }
}

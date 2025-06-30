using SilverLeaf.Entities.DTOs;
using System.Threading.Tasks;

namespace SilverLeaf.Screener.Admin.Services.Interfaces
{
    public interface IStudentService
    {
        StudentDTO Student { get; }

        Task Register(StudentDTO student);

        void ClearStudentCache();
    }
}

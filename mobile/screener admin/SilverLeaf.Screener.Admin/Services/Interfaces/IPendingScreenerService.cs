using SilverLeaf.Common.Models;
using SilverLeaf.Entities.DTOs;
using System;
using System.Threading.Tasks;

namespace SilverLeaf.Screener.Admin.Services.Interfaces
{
    public interface IPendingScreenerService
    {
        AdjustableDTO<StudentDTO> PendingStudentScreeners { get; set; }

        Task Load();

        event EventHandler<AdjustableDTO<StudentDTO>> PendingStudentScreenersChanged;
    }
}

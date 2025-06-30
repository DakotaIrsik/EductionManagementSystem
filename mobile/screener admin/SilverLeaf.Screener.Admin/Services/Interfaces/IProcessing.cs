using SilverLeaf.Screener.Admin.Objects;
using System;
using System.Threading.Tasks;

namespace SilverLeaf.Screener.Admin.Services.Interfaces
{
    public interface IProcessing
    {
        Response Process(Action action);
        Task<Response> Process(Task task, string loadingMessage = null);
        Task<Response<T>> Process<T>(Task<T> task, string loadingMessage = null);
    }
}

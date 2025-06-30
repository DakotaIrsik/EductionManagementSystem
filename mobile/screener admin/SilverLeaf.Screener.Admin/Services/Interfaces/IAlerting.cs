using System.Threading.Tasks;

namespace SilverLeaf.Screener.Admin.Services.Interfaces
{
    public interface IAlerting
    {
        Task Show(string title, string message, string button);
        Task<bool> Show(string title, string message, string buttonPositive, string buttonNegative);
        Task ShowDefaultWarning(string message);
    }
}

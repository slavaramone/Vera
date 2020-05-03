using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ISmsSender
    {
        Task<bool> SendAsync(string phone, string message);
    }
}

using System.Threading.Tasks;

namespace ServiceExtensions.PmsAdapter.PmsProcessor
{
    public interface IPmsProcessorService
    {
        Task<bool> SubmitMessage(string username, string password, string messageString, string hotelId);
    }
}

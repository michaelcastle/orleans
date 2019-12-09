using OutboundAdapter.Interfaces.Models;
using System.Threading.Tasks;

namespace OutboundAdapter.Interfaces
{
    public interface IHotelPmsGrain : Orleans.IGrainWithIntegerKey
    {
        Task<string> StreamNamespace<T>();
        Task<bool> IsConnected();
        Task<HotelConfiguration> GetOutboundConfiguration();
        Task IncrementAsync();
        Task SaveOutboundConfigurationAsync(HotelConfiguration configuration);
    }
}

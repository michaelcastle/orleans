using OutboundAdapter.Interfaces.Models;
using System.Threading.Tasks;

namespace OutboundAdapter.Interfaces.Consumer
{
    public interface IInboundConsumerGrain : Orleans.IGrainWithIntegerKey
    {
        Task<ISubscribeToResponseObserver> GetInboundConsumer(InboundConfiguration configuration, int hotelId);
    }
}

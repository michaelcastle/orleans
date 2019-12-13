using Orleans;
using Orleans.Streams;
using OutboundAdapter.Interfaces.Models;
using System.Threading.Tasks;

namespace OutboundAdapter.Interfaces
{
    public interface ISubscribeToResponseObserver : IAsyncObserver<string>, IGrainWithGuidKey
    {
        Task SetConfiguration(InboundConfiguration configuration);
    }
}

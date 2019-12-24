using Orleans;
using Orleans.Streams;
using OutboundAdapter.Interfaces.Models;
using System;
using System.Threading.Tasks;

namespace OutboundAdapter.Interfaces
{
    public interface ISubscribeToResponseObserver : IAsyncObserver<string>, ISubscribeObserver
    {
    }

    public interface ISubscribeObserver : IGrainWithIntegerCompoundKey
    {
        Task SetConfiguration(ISubscribeEndpoint configuration);
        Task BecomeConsumer(Guid streamId, string streamNamespace, string providerToUse);
        Task StopConsuming();
    }

    public interface ISubscribeWithNamespaceObserver : ISubscribeObserver
    {
        Task<string> Namespace();
    }
}

using Orleans;

namespace OutboundAdapter.Interfaces.Consumer
{
    public interface ISubmitMessageConsumer : ISubscribeToResponseObserver, IGrainWithIntegerKey
    {
    }
}

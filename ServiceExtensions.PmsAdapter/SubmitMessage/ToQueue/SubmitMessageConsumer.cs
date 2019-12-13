using MassTransit;
using ServiceExtensions.PmsAdapter.ClientChannel;
using ServiceExtensions.PmsAdapter.Connected_Services.PmsProcessor;
using ServiceExtensions.PmsAdapter.PmsProcessor;
using System.Threading.Tasks;

namespace ServiceExtensions.PmsAdapter.SubmitMessage.ToQueue
{
    public class SubmitMessageConsumer : IConsumer<SubmitMessage>
    {
        private readonly IPmsProcessorService _pmsProcessorService;
        private readonly IClientChannelFactory<IPMSInterfaceContractChannel> _clientFactory;

        public SubmitMessageConsumer(IPmsProcessorService pmsProcessorService, IClientChannelFactory<IPMSInterfaceContractChannel> clientFactory)
        {
            _pmsProcessorService = pmsProcessorService;
            _clientFactory = clientFactory;
        }

        public async Task Consume(ConsumeContext<SubmitMessage> context)
        {
            await _pmsProcessorService.SubmitMessage(_clientFactory, context.Message.Username, context.Message.Password, context.Message.Message);
        }
    }
}

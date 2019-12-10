using MassTransit;
using ServiceExtensions.PmsAdapter.PmsProcessor;
using System.Threading.Tasks;

namespace ServiceExtensions.PmsAdapter.SubmitMessage.ToQueue
{
    public class SubmitMessageConsumer : IConsumer<SubmitMessage>
    {
        private readonly IPmsProcessorService _pmsProcessorService;

        public SubmitMessageConsumer(IPmsProcessorService pmsProcessorService)
        {
            _pmsProcessorService = pmsProcessorService;
        }

        public async Task Consume(ConsumeContext<SubmitMessage> context)
        {
            await _pmsProcessorService.SubmitMessage(context.Message.Username, context.Message.Password, context.Message.Message, context.Message.HotelId);
        }
    }
}

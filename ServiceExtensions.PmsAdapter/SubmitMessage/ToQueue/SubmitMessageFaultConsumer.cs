using MassTransit;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ServiceExtensions.PmsAdapter.SubmitMessage.ToQueue
{
    public class SubmitMessageFaultConsumer : IConsumer<Fault<SubmitMessage>>
    {
        private readonly ILogger<SubmitMessageFaultConsumer> _logger;

        public SubmitMessageFaultConsumer(ILogger<SubmitMessageFaultConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<Fault<SubmitMessage>> context)
        {
            var originalMessage = context.Message.Message;
            var exceptions = context.Message.Exceptions;

            foreach(var exception in exceptions)
            {
                _logger.LogError(exception.Message, originalMessage);
            }

            return Task.CompletedTask;
        }
    }
}

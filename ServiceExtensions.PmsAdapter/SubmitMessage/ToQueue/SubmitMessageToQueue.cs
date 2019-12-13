using MassTransit;
using Microsoft.Extensions.Logging;
using ServiceExtensions.PmsAdapter.PmsProcessor;
using Polly;
using System;
using System.Threading.Tasks;
using ServiceExtensions.PmsAdapter.SubmitMessage.Direct;
using ServiceExtensions.PmsAdapter.ClientChannel;
using ServiceExtensions.PmsAdapter.Connected_Services.PmsProcessor;

namespace ServiceExtensions.PmsAdapter.SubmitMessage.ToQueue
{
    public class SubmitMessageToQueue : ISubmitMessageHandler
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<ISubmitMessageHandler> _logger;
        private readonly ISubmitMessageHandler _submitMessageDirect;

        public SubmitMessageToQueue(IPublishEndpoint publishEndpoint, ILogger<ISubmitMessageHandler> logger, IPmsProcessorService pmsProcessorService, IClientChannelFactory<IPMSInterfaceContractChannel> clientFactory)
        {
            _publishEndpoint = publishEndpoint;
            _logger = logger;

            _submitMessageDirect = new InboundPmsAdapter(logger, pmsProcessorService, clientFactory);
        }

        public async Task<SubmitMessageResponse> Submit(SubmitMessage submitMessage)
        {
            _logger.LogDebug("SubmitMessageToQueue: {submitMessage}", submitMessage.Message);

            try
            {
                // If it cannot connect to rabbitmq then fallback to trying to access the endpoint directly
                // Try and access rabbitmq 3 times before giving up
                return await FallbackPolicy(submitMessage)
                    .WrapAsync(RetryPolicy())
                    .ExecuteAsync(async () =>
                    {
                        await _publishEndpoint.Publish(submitMessage);
                        return new SubmitMessageResponse
                        {
                            IsSuccessful = true
                        };
                    });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fail publish to optii: {body}", submitMessage.Message);
                return new SubmitMessageResponse
                {
                    IsSuccessful = true,
                    FailReason = ex
                };
            }
        }

        private Polly.Fallback.AsyncFallbackPolicy<SubmitMessageResponse> FallbackPolicy(SubmitMessage submitMessage)
        {
            return Policy<SubmitMessageResponse>
                .HandleInner<MassTransitException>()
                .Or<RabbitMQ.Client.Exceptions.BrokerUnreachableException>()
                .FallbackAsync(unused => _submitMessageDirect.Submit(submitMessage));
        }

        private Polly.Retry.AsyncRetryPolicy RetryPolicy()
        {
            return Policy
                .HandleInner<MassTransitException>()
                .Or<RabbitMQ.Client.Exceptions.BrokerUnreachableException>()
                .RetryAsync(3, onRetry: (exception, retryCount, context) =>
                {
                    Console.WriteLine($"Retry {retryCount} of {context.PolicyKey}, due to: {exception}.");
                });
        }
    }
}

using MassTransit;
using MassTransit.RabbitMqTransport;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace ServiceExtensions.ServiceBus
{
    public class PublishObserver : IPublishObserver
    {
        private readonly ILogger<PublishObserver> _logger;

        public PublishObserver(ILogger<PublishObserver> logger)
        {
            _logger = logger;
        }

        public Task PrePublish<T>(PublishContext<T> context)
            where T : class
        {
            // called right before the message is published (sent to exchange or topic)
            _logger.LogDebug($"Publish To Queue: MessageId:[{context.MessageId}], DestinationAddress:[{context.DestinationAddress}], SourceAddress:[{context.SourceAddress}]", context.MessageId, context.DestinationAddress, context.SourceAddress);

            return Task.CompletedTask;
        }

        public Task PostPublish<T>(PublishContext<T> context)
            where T : class
        {
            // called after the message is published (and acked by the broker if RabbitMQ)
            _logger.LogDebug($"Publish To Queue: SUCCESS: {context.Message}");

            return Task.CompletedTask;
        }

        public Task PublishFault<T>(PublishContext<T> context, Exception exception)
            where T : class
        {
            // called if there was an exception publishing the message
            if (exception.InnerException is RabbitMqConnectionException)
            {
                _logger.LogError($"Publish To Queue: InnerException = [RabbitMQ.Client.Exceptions.RabbitMqConnectionException: {exception.Message}]");
            }
            if (exception.InnerException is RabbitMQ.Client.Exceptions.BrokerUnreachableException)
            {
                _logger.LogError($"Publish To Queue: InnerException = [RabbitMQ.Client.Exceptions.BrokerUnreachableException: {exception.Message}]");
            }

            return Task.CompletedTask;
        }
    }
}

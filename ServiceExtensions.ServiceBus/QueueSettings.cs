namespace ServiceExtensions.ServiceBus
{
    public class QueueSettings
    {
        public QueueType QueueType { get; set; }
        public string QueueName { get; set; }
        public RabbitMqSettings RabbitMqSettings { get; set; }
    }

    public enum QueueType
    {
        InMemory,
        RabbitMq
    }
}

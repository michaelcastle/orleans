namespace ServiceExtensions.ServiceBus
{
    public class RabbitMqSettings : MassTransit.RabbitMqTransport.Hosting.RabbitMqSettings
    {
        public string Host { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }


        public ushort? Heartbeat { get; set; }

        public int? Port { get; set; }

        public string VirtualHost { get; set; }

        public string ClusterMembers { get; set; }
    }
}

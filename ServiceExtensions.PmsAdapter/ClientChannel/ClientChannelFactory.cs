using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;

namespace ServiceExtensions.PmsAdapter.ClientChannel
{
    public class ClientChannelFactory<TChannel> : IClientChannelFactory<TChannel>
        where TChannel : class, ICommunicationObject
    {
        private readonly ChannelFactory<TChannel> _factory;

        public ClientCredentials Credentials => _factory.Credentials;

        public ServiceEndpoint EndPoint => _factory.Endpoint;

        public ClientChannelFactory(Binding binding, EndpointAddress remoteAddress)
        {
            _factory = new ChannelFactory<TChannel>(binding, remoteAddress);
        }

        public void Open()
        {
            _factory.Open();
        }

        public void Close()
        {
            _factory.Close();
        }

        public void Abort()
        {
            _factory.Abort();
        }

        public TChannel CreateChannel()
        {
            return _factory.CreateChannel();
        }

        public void CloseChannel(TChannel client)
        {
            if (client == null)
            {
                return;
            }

            try
            {
                if (client.State != CommunicationState.Faulted)
                {
                    client.Close();
                }
            }
            finally
            {
                if (client.State != CommunicationState.Closed)
                {
                    client.Abort();
                }
            }
        }
    }
}

using System.ServiceModel.Description;

namespace ServiceExtensions.PmsAdapter.ClientChannel
{
    public interface IClientChannelFactory<TChannel>
    {
        ClientCredentials Credentials { get; }
        ServiceEndpoint EndPoint { get; }

        void Open();
        void Close();
        void Abort();
        TChannel CreateChannel();
        void CloseChannel(TChannel client);
    }
}

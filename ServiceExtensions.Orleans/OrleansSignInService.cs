using Orleans;
using OutboundAdapter.Interfaces;
using OutboundAdapter.Interfaces.Models;
using ServiceExtensions.PmsAdapter.ClientChannel;
using ServiceExtensions.PmsAdapter.Connected_Services.PmsProcessor;
using ServiceExtensions.PmsAdapter.SignIn;
using System;
using System.ServiceModel;

namespace ServiceExtensions.Orleans
{
    public class OrleansSignInService : ISessionItemAuthenticationService
    {
        private readonly IClusterClient _clusterClient;
        private IClientChannelFactory<IPMSInterfaceContractChannel> _clientFactory;

        public OrleansSignInService(IClusterClient clusterClient)
        {
            _clusterClient = clusterClient;
            _clientFactory = clientFactory;
        }

        private ClientChannelFactory<IPMSInterfaceContractChannel> GetHttpClientFactory(EndpointAddress endpoint)
        {
            var binding = new BasicHttpBinding();

            return new ClientChannelFactory<IPMSInterfaceContractChannel>(binding, endpoint);
        }

        private ClientChannelFactory<IPMSInterfaceContractChannel> GetHttpsClientFactory(EndpointAddress endpoint)
        {
            var binding = new BasicHttpsBinding(BasicHttpsSecurityMode.Transport);
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;

            var clientFactory = new ClientChannelFactory<IPMSInterfaceContractChannel>(binding, endpoint);

            return clientFactory;
        }

        public SessionItem SignIn(string username, string password, string lastAction, string hotelId)
        {
            int.TryParse(hotelId, out int hotelIdInt);
            var hotel = _clusterClient.GetGrain<IHotelPmsGrain>(hotelIdInt);
            if (!hotel.IsInboundConnected().Result)
            {
                return null;
            }

            ValidateClientFactory(hotel);

            var client = _clientFactory.CreateChannel();
            try
            {
                var config = hotel.GetInboundConfiguration().Result;
                var signinRequest = new SigninRequest(config.Credentials.EncryptedUsername, config.Credentials.EncryptedPassword);
                var signin = client.SigninAsync(signinRequest).Result;

                if (signin.InterfaceSigninValues == null)
                {
                    return null;
                }

                return new SessionItem()
                {
                    SessionId = signin.InterfaceSigninValues.SessionId,
                    UserName = username,
                    IsAuthorised = true
                };
            }
            finally
            {
                _clientFactory.CloseChannel(client);
            }
        }

        private void ValidateClientFactory(IHotelPmsGrain hotel)
        {
            var config = hotel.GetInboundConfiguration().Result;
            var endpoint = new EndpointAddress(new Uri(config.Url));
            if (endpoint.Uri.AbsoluteUri != _clientFactory.EndPoint.Address.Uri.AbsoluteUri)
            {
                _clientFactory = endpoint.Uri.Scheme == "http" ? GetHttpClientFactory(endpoint) : GetHttpsClientFactory(endpoint);
            }
        }
    }
}

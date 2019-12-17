using Orleans;
using Orleans.Streams;
using OutboundAdapter.Interfaces.Consumer;
using OutboundAdapter.Interfaces.Models;
using ServiceExtensions.PmsAdapter.ClientChannel;
using ServiceExtensions.PmsAdapter.Connected_Services.PmsProcessor;
using ServiceExtensions.PmsAdapter.PmsProcessor;
using System;
using System.ServiceModel;
using System.Threading.Tasks;

namespace OutboundAdapter.Grains.V2
{
    public class SubmitMessageConsumer : Grain, ISubmitMessageConsumer
    {
        private InboundConfiguration _configuration;
        private readonly IPmsProcessorService _pmsProcessorService;
        private IClientChannelFactory<IPMSInterfaceContractChannel> _clientFactory;
        private StreamSubscriptionHandle<string> _subscription;

        public SubmitMessageConsumer(IPmsProcessorService pmsProcessorService)
        {
            _pmsProcessorService = pmsProcessorService;
        }

        public async Task OnNextAsync(string message, StreamSequenceToken token = null)
        {
            await _pmsProcessorService.SubmitMessage(_clientFactory, _configuration.Credentials.EncryptedUsername, _configuration.Credentials.EncryptedPassword, message);
        }

        public Task OnCompletedAsync()
        {
            return Task.CompletedTask;
        }

        public Task OnErrorAsync(Exception ex)
        {
            return Task.CompletedTask;
        }

        public Task SetConfiguration(InboundConfiguration configuration)
        {
            _configuration = configuration;
            _clientFactory = SetClientFactory(_configuration);
            return Task.CompletedTask;
        }

        public async Task BecomeConsumer(Guid streamId, string streamNamespace, string providerToUse)
        {
            var streamProvider = GetStreamProvider(providerToUse);
            var stream = streamProvider.GetStream<string>(streamId, streamNamespace);
            _subscription = await stream.SubscribeAsync(OnNextAsync, OnErrorAsync, OnActivateAsync);
        }

        private IClientChannelFactory<IPMSInterfaceContractChannel> SetClientFactory(InboundConfiguration configuration)
        {
            if (string.IsNullOrEmpty(configuration.Url))
            {
                throw new Exception("No url defined");
            }

            var endpoint = new EndpointAddress(new Uri(configuration.Url));
            if (endpoint.Uri.AbsoluteUri == _clientFactory?.EndPoint?.Address?.Uri?.AbsoluteUri)
            {
                return _clientFactory;
            }

            return endpoint.Uri.Scheme == "http" ? GetHttpClientFactory(endpoint) : GetHttpsClientFactory(endpoint);
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

        public async Task StopConsuming()
        {
            if (_subscription != null)
            {
                await _subscription.UnsubscribeAsync();
                _subscription = null;
            }
        }
    }
}

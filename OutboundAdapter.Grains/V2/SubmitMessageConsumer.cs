using Orleans.Runtime;
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
    public class SubmitMessageConsumer : SubscribeObserver<string>, ISubmitMessageConsumer
    {
        private readonly IPmsProcessorService _pmsProcessorService;
        private IClientChannelFactory<IPMSInterfaceContractChannel> _clientFactory;

        public SubmitMessageConsumer([PersistentState("subscribeEndpointConfiguration", "subscribeEndpointConfigurationStore")] IPersistentState<SubscribeEndpoint> configuration, IPmsProcessorService pmsProcessorService) : base(configuration)
        {
            _pmsProcessorService = pmsProcessorService;
        }

        public override async Task OnActivateAsync()
        {
            if (_configuration.State.Url != null)
            {
                await SetConfiguration(_configuration.State);
                await base.OnActivateAsync();
            }
        }

        public override async Task OnNextAsync(string message, StreamSequenceToken token = null)
        {
            await _pmsProcessorService.SubmitMessage(_clientFactory, _configuration.State.Credentials.EncryptedUsername, _configuration.State.Credentials.EncryptedPassword, message);
        }

        //public override Task OnCompletedAsync()
        //{
        //    return Task.CompletedTask;
        //}

        //public override Task OnErrorAsync(Exception ex)
        //{
        //    return Task.CompletedTask;
        //}

        public new async Task SetConfiguration(ISubscribeEndpoint configuration)
        {
            var saveTask = base.SetConfiguration(configuration);
            _clientFactory = SetClientFactory(configuration);
            await saveTask;
        }

        //public async Task BecomeConsumer(Guid streamId, string streamNamespace, string providerToUse)
        //{
        //    var streamProvider = GetStreamProvider(providerToUse);
        //    var stream = streamProvider.GetStream<string>(streamId, streamNamespace);
        //    _subscription = await stream.SubscribeAsync(OnNextAsync, OnErrorAsync, OnActivateAsync);
        //}

        private IClientChannelFactory<IPMSInterfaceContractChannel> SetClientFactory(ISubscribeEndpoint configuration)
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

        //public async Task StopConsuming()
        //{
        //    if (_subscription != null)
        //    {
        //        await _subscription.UnsubscribeAsync();
        //        _subscription = null;
        //    }
        //}
    }
}

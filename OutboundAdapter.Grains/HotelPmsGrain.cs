using Orleans;
using OutboundAdapter.Interfaces;
using OutboundAdapter.Interfaces.Models;
using System.Threading.Tasks;
using Orleans.Runtime;
using static OutboundAdapter.Interfaces.Opera.Constants;
using Orleans.Streams;
using System.Collections.Generic;

namespace OutboundAdapter.Grains
{
    public class HotelPmsGrain : Grain, IHotelPmsGrain
    {
        private readonly IPersistentState<HotelConfiguration> _hotelConfiguration;

        private readonly List<IAsyncStream<string>> _consumers = new List<IAsyncStream<string>>();
        private readonly List<StreamSubscriptionHandle<string>> consumerHandles = new List<StreamSubscriptionHandle<string>>();

        Task<bool> IHotelPmsGrain.IsOutboundConnected()
        {
            return Task.FromResult(_hotelConfiguration.State.OutboundConfiguration != null && !string.IsNullOrEmpty(_hotelConfiguration.State.OutboundConfiguration.PmsType));
        }

        Task<bool> IHotelPmsGrain.IsInboundConnected()
        {
            return Task.FromResult(_hotelConfiguration.State.InboundConfiguration != null && !string.IsNullOrEmpty(_hotelConfiguration.State.InboundConfiguration.InboundType));
        }

        public HotelPmsGrain([PersistentState("hotelConfiguration", "hotelConfigurationStore")] IPersistentState<HotelConfiguration> hotelConfiguration)
        {
            _hotelConfiguration = hotelConfiguration;
        }

        public override async Task OnActivateAsync()
        {
            await base.OnActivateAsync();
        }

        Task<InboundConfiguration> IHotelPmsGrain.GetInboundConfiguration()
        {
            return Task.FromResult(_hotelConfiguration.State.InboundConfiguration);
        }

        Task<OutboundConfiguration> IHotelPmsGrain.GetOutboundConfiguration()
        {
            return Task.FromResult(_hotelConfiguration.State.OutboundConfiguration);
        }

        async Task IHotelPmsGrain.IncrementAsync()
        {
            _hotelConfiguration.State.TotalNumber++;
            await _hotelConfiguration.WriteStateAsync();
        }

        async Task IHotelPmsGrain.SaveOutboundConfigurationAsync(OutboundConfiguration configuration)
        {
            _hotelConfiguration.State.OutboundConfiguration = configuration;
            await _hotelConfiguration.WriteStateAsync();
        }

        // TODO: Change this to streaming and subscribing multiple receivers to input
        async Task IHotelPmsGrain.SubscribeToResponses(InboundConfiguration configuration, string provider, IList<string> streamNamespaces, ISubscribeToResponseObserver observer)
        {
            _hotelConfiguration.State.InboundConfiguration = configuration;

            IStreamProvider streamProvider = base.GetStreamProvider(provider);
            foreach (var streamNamespace in streamNamespaces)
            {
                var stream = streamProvider.GetStream<string>(this.GetPrimaryKey(), streamNamespace);
                _consumers.Add(stream);
                await observer.SetConfiguration(configuration);
                consumerHandles.Add(await stream.SubscribeAsync(observer));
            }

            await _hotelConfiguration.WriteStateAsync();
        }

        public Task<string> StreamNamespaceOutbound<T>()
        {
            return Task.FromResult($"{nameof(Outbound)}{typeof(T).Name}{_hotelConfiguration.State.OutboundConfiguration.PmsType}");
        }

        public Task<string> StreamNamespaceInbound<T, P>()
        {
            return Task.FromResult($"{nameof(Inbound)}{typeof(T).Name}{typeof(P).Name}");
        }
    }
}

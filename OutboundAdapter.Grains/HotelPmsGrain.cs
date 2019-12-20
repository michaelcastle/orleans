using Orleans;
using Orleans.Runtime;
using OutboundAdapter.Interfaces;
using OutboundAdapter.Interfaces.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OutboundAdapter.Grains
{
    public class HotelPmsGrain : Grain, IHotelPmsGrain
    {
        private readonly IPersistentState<HotelConfiguration> _hotelConfiguration;
        private readonly Dictionary<string, ISubscribeToResponseObserver> _inboundSubscribers = new Dictionary<string, ISubscribeToResponseObserver>();

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

        Task<OutboundConfiguration> IHotelPmsGrain.GetOutboundConfiguration()
        {
            return Task.FromResult(_hotelConfiguration.State.OutboundConfiguration);
        }

        async Task IHotelPmsGrain.SaveConsumerConfigurationAsync(OutboundConfiguration configuration)
        {
            _hotelConfiguration.State.OutboundConfiguration = configuration;
            await _hotelConfiguration.WriteStateAsync();
        }

        async Task IHotelPmsGrain.SubscribeToInbound(InboundConfiguration configuration, string provider, IList<string> streamNamespaces, ISubscribeToResponseObserver observer)
        {
            var config = observer.SetConfiguration(configuration);

            var tasks = new List<Task>();

            await config;

            foreach (var streamNamespace in streamNamespaces)
            {
                tasks.Add(observer.BecomeConsumer(this.GetPrimaryKey(), streamNamespace, provider));
                if (_inboundSubscribers.ContainsKey(configuration.Key()))
                {
                    await Unsubscribe(configuration);
                }

                _inboundSubscribers.Add(configuration.Key(), observer);
            }

            await Task.WhenAll(tasks);

            await _hotelConfiguration.WriteStateAsync();
        }

        public async Task Unsubscribe(InboundConfiguration configuration)
        {
            _inboundSubscribers.TryGetValue(configuration.Key(), out var observer);
            if (observer == null)
            {
                return;
            }
            await observer.StopConsuming();
            _inboundSubscribers.Remove(configuration.Key());
        }
    }
}

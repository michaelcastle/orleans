using Orleans;
using OutboundAdapter.Interfaces;
using OutboundAdapter.Interfaces.Models;
using System.Threading.Tasks;
using Orleans.Runtime;

namespace OutboundAdapter.Grains
{
    public class HotelPmsGrain : Grain, IHotelPmsGrain
    {
        private readonly IPersistentState<HotelConfiguration> _hotelConfiguration;

        Task<bool> IHotelPmsGrain.IsConnected()
        {
            return Task.FromResult(_hotelConfiguration.State != null && _hotelConfiguration.State.PmsType != 0);
        }

        public HotelPmsGrain([PersistentState("hotelConfiguration", "hotelConfigurationStore")] IPersistentState<HotelConfiguration> hotelConfiguration)
        {
            _hotelConfiguration = hotelConfiguration;
        }

        public override async Task OnActivateAsync()
        {
            await base.OnActivateAsync();
        }

        Task<HotelConfiguration> IHotelPmsGrain.GetOutboundConfiguration()
        {
            return Task.FromResult(_hotelConfiguration.State);
        }

        async Task IHotelPmsGrain.IncrementAsync()
        {
            _hotelConfiguration.State.TotalNumber++;
            await _hotelConfiguration.WriteStateAsync();
        }

        async Task IHotelPmsGrain.SaveOutboundConfigurationAsync(HotelConfiguration configuration)
        {
            _hotelConfiguration.State = configuration;
            await _hotelConfiguration.WriteStateAsync();
        }
    }
}

using Orleans;
using OutboundAdapter.Interfaces;
using OutboundAdapter.Interfaces.Models;
using OutboundAdapter.Interfaces.PmsClients;
using System.Threading.Tasks;
using Orleans.Runtime;

namespace OutboundAdapter.Grains
{
    public class HotelPmsGrain : Grain, IHotelPmsGrain
    {
        private IOutboundMappingGrains _hotelOutboundMapper;
        private readonly IHttpFactoryGrain _httpFactoryGrain;

        private readonly IPersistentState<HotelConfiguration> _hotelConfiguration;

        private long GrainKey => this.GetPrimaryKeyLong();

        // TODO: Hardcoded for now
        public HotelPmsGrain([PersistentState("hotelConfiguration", "hotelConfigurationStore")] IPersistentState<HotelConfiguration> hotelConfiguration)
        {
            _hotelConfiguration = hotelConfiguration;
            //    new HotelConfiguration
            //{
            //    PmsType = 1,
            //    Url = "https://ove-osb.microsdc.us:9015"
            //};
            //_httpFactoryGrain = new HttpFactoryOperaGrain(htng2008bClient, htng2008BExtClient);
        }

        public override async Task OnActivateAsync()
        {
            var pmsType = await Task.FromResult(_hotelConfiguration.State.PmsType);
            switch (pmsType)
            {
                case 1:
                    _hotelOutboundMapper = GrainFactory.GetGrain<IOutboundMappingGrains>(this.GetPrimaryKeyLong());
                    break;

                default:
                    break;
            }

            await base.OnActivateAsync();
        }

        Task<HotelConfiguration> IHotelPmsGrain.Get()
        {
            return Task.FromResult(_hotelConfiguration.State);
        }

        Task<IOutboundMappingGrains> IHotelPmsGrain.GetOutboundMapper()
        {
            return Task.FromResult(_hotelOutboundMapper);
        }

        Task<IHttpFactoryGrain> IHotelPmsGrain.GetClient()
        {
            return Task.FromResult(_httpFactoryGrain);
        }

        async Task IHotelPmsGrain.IncrementAsync()
        {
            _hotelConfiguration.State.TotalNumber++;
            await _hotelConfiguration.WriteStateAsync();
        }
    }
}

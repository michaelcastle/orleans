using Orleans;
using Orleans.Streams;
using OutboundAdapter.Interfaces;
using OutboundAdapter.Interfaces.Models;
using OutboundAdapter.Interfaces.Opera;
using OutboundAdapter.Interfaces.Opera.Inbound;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OutboundAdapter.Grains.Opera
{
    [ImplicitStreamSubscription(Constants.Inbound.RoomStatusUpdateStream)]
    public class RoomStatusUpdateConsumer : Grain, IGrainWithIntegerKey, IRoomStatusUpdateConsumer
    {
        private readonly List<IAsyncStream<string>> _streams = new List<IAsyncStream<string>>();
        private readonly List<StreamSubscriptionHandle<string>> _handles = new List<StreamSubscriptionHandle<string>>();
        private IStreamProvider _streamProvider;
        private readonly IClusterClient _clusterClient;

        private int HotelId => (int)this.GetPrimaryKeyLong();

        public RoomStatusUpdateConsumer(IClusterClient clusterClient)
        {
            _clusterClient = clusterClient;
        }

        public override async Task OnActivateAsync()
        {
            _streamProvider = GetStreamProvider("SMSProvider");

            var stream = _streamProvider.GetStream<string>(this.GetPrimaryKey(), Constants.Inbound.RoomStatusUpdateStream);
            _streams.Add(stream);
            _handles.Add(await stream.SubscribeAsync(OnNextAsync, OnErrorAsync, OnCompletedAsync));

            await base.OnActivateAsync();
        }

        public Task OnCompletedAsync()  
        {
            throw new NotImplementedException();
        }

        public Task OnErrorAsync(Exception ex)
        {
            throw new NotImplementedException();
        }

        public async Task OnNextAsync(string content, StreamSequenceToken token = null)
        {
            var hotel = _clusterClient.GetGrain<IHotelPmsGrain>(HotelId);
            if (!await hotel.IsConnected())
            {
                throw new Exception("Hotel is not connected to the PMS. Please Connect first.");
            }

            var mapper = _clusterClient.GetGrain<IInboundMappingGrains>(HotelId);
            var request = mapper.MapRoomStatusUpdateBE(content);

            var hotelGrain = _clusterClient.GetGrain<IInboundAdapterGrain>(HotelId);
            var requestDto = await request;
            var response = hotelGrain.RoomStatusUpdate(1, new RoomStatusUpdate
            {
                ResortId = requestDto.ResortId,
                RoomNumber = requestDto.RoomNumber,
                RoomStatus = requestDto.RoomStatus,
                RoomType = requestDto.RoomType
            });

            // TODO: Send response to Optii, however that is done.
        }
    }
}

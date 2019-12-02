using Orleans;
using Orleans.Concurrency;
using OutboundAdapter.Interfaces;
using OutboundAdapter.Interfaces.Models;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace OutboundAdapter.Grains
{
    [StatelessWorker]
    public class OutboundMappingOperaGrains : Grain, IOutboundMappingGrains
    {
        private int _hotelId;

        Task<string> IOutboundMappingGrains.MapFetchProfile(FetchProfile request)
        {
            throw new NotImplementedException();
        }

        Task<string> IOutboundMappingGrains.MapFetchReservation(FetchReservation request)
        {
            throw new NotImplementedException();
        }

        Task<string> IOutboundMappingGrains.MapReservationLookup(ReservationLookup request)
        {
            throw new NotImplementedException();
        }

        Task<string> IOutboundMappingGrains.MapUpdateRoomStatus(UpdateRoomStatusRequest request)
        {
            // Convert from the source XML to the Destination request
            request.RoomStatus = "VC";

            var content = JsonSerializer.Serialize(request);

            return Task.FromResult(content);
        }
    }
}

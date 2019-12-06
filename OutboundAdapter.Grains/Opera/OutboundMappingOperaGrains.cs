using Orleans;
using Orleans.Concurrency;
using OutboundAdapter.Interfaces;
using OutboundAdapter.Interfaces.Models;
using System;
using OutboundAdapter.Interfaces.Models.Opera;
using System.Threading.Tasks;
using System.Xml;
using OutboundAdapter.Interfaces.PmsClients;

namespace OutboundAdapter.Grains.Opera
{
    [StatelessWorker]
    public class OutboundMappingOperaGrains : Grain, IOutboundMappingGrains
    {
        private int _hotelId;
        private readonly IOperaEnvelopeSerializer _operaRequestSerializer;

        public OutboundMappingOperaGrains(IOperaEnvelopeSerializer operaRequestSerializer)
        {
            _operaRequestSerializer = operaRequestSerializer;
        }

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

        async Task<string> IOutboundMappingGrains.MapUpdateRoomStatus(string request)
        {
            return await Task.Run(() => {
                // Convert from the source XML to the Destination request
                var requestBody = _operaRequestSerializer.Deserialize<UpdateRoomStatusRequestDto>(request);
//                    @"
//<UpdateRoomStatusRequest xmlns=""http://webservices.micros.com/htng/2008B/SingleGuestItinerary/Housekeeping/Types"">
//    <ResortId>USOWS</ResortId>
//    <RoomNumber>0105</RoomNumber>
//    <RoomStatus>Dirty</RoomStatus>
//</UpdateRoomStatusRequest>
//");
                var envelope = new UpdateRoomStatusRequestEnvelopeDto
                {
                    Header = _operaRequestSerializer.GetHeaderRequest("http://webservices.micros.com/htng/2008B/SingleGuestItinerary#UpdateRoomStatus"),
                    Body = new UpdateRoomStatusRequestBodyDto
                    {
                        Request = requestBody
                    }
                };

                var namespaces = new[] {
                    new XmlQualifiedName("", "http://webservices.micros.com/htng/2008B/SingleGuestItinerary/Housekeeping/Types")
                };
                var content = _operaRequestSerializer.Serialize(envelope, namespaces);
                return content;
            });
        }
    }
}

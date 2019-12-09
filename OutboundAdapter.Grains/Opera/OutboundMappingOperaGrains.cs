using Orleans;
using Orleans.Concurrency;
using OutboundAdapter.Interfaces.Models;
using System;
using OutboundAdapter.Interfaces.Opera.Models;
using System.Threading.Tasks;
using System.Xml;
using OutboundAdapter.Interfaces.Opera;

namespace OutboundAdapter.Grains.Opera
{
    [StatelessWorker]
    public class OutboundMappingOperaGrains : Grain, IOutboundMappingGrains
    {
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

        async Task<string> IOutboundMappingGrains.MapUpdateRoomStatus(UpdateRoomStatusRequestDto request)
        {
            return await Task.Run(() => {
                var envelope = new UpdateRoomStatusRequestEnvelopeDto
                {
                    Header = _operaRequestSerializer.GetHeaderRequest("http://webservices.micros.com/htng/2008B/SingleGuestItinerary#UpdateRoomStatus"),
                    Body = new UpdateRoomStatusRequestBodyDto
                    {
                        Request = request
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

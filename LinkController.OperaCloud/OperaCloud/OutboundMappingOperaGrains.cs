using Orleans;
using Orleans.Concurrency;
using OutboundAdapter.Interfaces.Models;
using System;
using System.Threading.Tasks;
using System.Xml;
using LinkController.OperaCloud.Interfaces.Outbound;
using LinkController.OperaCloud.Interfaces.Models;

namespace LinkController.OperaCloud.OperaCloud
{
    [StatelessWorker]
    public class OutboundMappingOperaGrains : Grain, IOutboundMappingOperaGrains
    {
        private readonly IOperaEnvelopeSerializer _operaRequestSerializer;

        public OutboundMappingOperaGrains(IOperaEnvelopeSerializer operaRequestSerializer)
        {
            _operaRequestSerializer = operaRequestSerializer;
        }

        Task<string> IOutboundMappingOperaGrains.MapFetchProfile(FetchProfile request)
        {
            throw new NotImplementedException();
        }

        Task<string> IOutboundMappingOperaGrains.MapFetchReservation(FetchReservation request)
        {
            throw new NotImplementedException();
        }

        Task<string> IOutboundMappingOperaGrains.MapReservationLookup(ReservationLookup request)
        {
            throw new NotImplementedException();
        }

        async Task<string> IOutboundMappingOperaGrains.MapUpdateRoomStatus(UpdateRoomStatusRequestDto request)
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

using LinkController.OperaCloud.Interfaces;
using LinkController.OperaCloud.Interfaces.Models;
using LinkController.OperaCloud.Interfaces.Outbound;
using Orleans;
using Orleans.Concurrency;
using OutboundAdapter.Interfaces.Models;
using System;
using System.Threading.Tasks;
using System.Xml;

namespace LinkController.OperaCloud.Grains.Mapping
{
    [StatelessWorker]
    public class OutboundMappingOperaCloudGrains : Grain, IOutboundMappingOperaCloudGrains
    {
        private readonly IOperaCloudEnvelopeSerializer _operaRequestSerializer;

        public OutboundMappingOperaCloudGrains(IOperaCloudEnvelopeSerializer operaRequestSerializer)
        {
            _operaRequestSerializer = operaRequestSerializer;
        }

        Task<string> IOutboundMappingOperaCloudGrains.MapFetchProfile(FetchProfile request)
        {
            throw new NotImplementedException();
        }

        Task<string> IOutboundMappingOperaCloudGrains.MapFetchReservation(FetchReservation request)
        {
            throw new NotImplementedException();
        }

        Task<string> IOutboundMappingOperaCloudGrains.MapReservationLookup(ReservationLookup request)
        {
            throw new NotImplementedException();
        }

        async Task<string> IOutboundMappingOperaCloudGrains.MapUpdateRoomStatus(UpdateRoomStatusRequestDto request)
        {
            return await Task.Run(() =>
            {
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

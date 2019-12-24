using LinkController.OperaCloud.Interfaces.Models;
using OutboundAdapter.Interfaces.StreamHelpers;
using System.Collections.Generic;

namespace LinkController.OperaCloud.Interfaces
{
    public static class Constants
    {
        public static class Outbound
        {
            public class OperaCloud
            {
                public const string UpdateRoomStatusRequestStream = nameof(Outbound) + nameof(UpdateRoomStatusRequestDto) + nameof(OperaCloud);
                //public const string FetchProfileStream = nameof(Outbound) + nameof(FetchProfile) + nameof(OperaCloud);
                //public const string FetchReservationStream = nameof(Outbound) + nameof(FetchReservation) + nameof(OperaCloud);
                //public const string ReservationLookupStream = nameof(Outbound) + nameof(ReservationLookup) + nameof(OperaCloud);
            }
        }

        public static class Inbound
        {
            public static class Htng
            {
                public const string RoomStatusUpdateStream = nameof(Inbound) + nameof(RoomStatusUpdateBERequestDto) + nameof(Htng);
                public const string UpdateRoomStatusResponseStream = nameof(Outbound) + nameof(UpdateRoomStatusResponseEnvelopeDto) + nameof(Htng);
            }

            public static class V2
            {
                public const string RoomStatusUpdateStream = nameof(Inbound) + nameof(RoomStatusUpdateBERequestDto) + nameof(V2);
                public const string UpdateRoomStatusResponseStream = nameof(Outbound) + nameof(UpdateRoomStatusResponseEnvelopeDto) + nameof(V2);
            }
        }

        public class V2StreamNamespaces : StreamNamespaces, IStreamV2Namespaces
        {
            public List<string> InboundNamespaces => new List<string>
            {
                InboundNamespace<RoomStatusUpdateBERequestDto, Outbound.OperaCloud>(),
                InboundNamespace<GuestStatusNotificationExtRequestDto, Outbound.OperaCloud>(),
                InboundNamespace<QueueRoomBERequestDto, Outbound.OperaCloud>(),
                InboundNamespace<NewProfileRequestDto, Outbound.OperaCloud>(),
                InboundNamespace<UpdateProfileRequestDto, Outbound.OperaCloud>(),
                OutboundNamespace<UpdateRoomStatusResponseEnvelopeDto, Outbound.OperaCloud>()
                //OutboundNamespace<FetchProfileResponse, Constants.Outbound.OperaCloud>()
                //OutboundNamespace<FetchReservationResponse, Constants.Outbound.OperaCloud>()
                //OutboundNamespace<ReservationLookupResponse, Constants.Outbound.OperaCloud>()
            };
        }
    }
}

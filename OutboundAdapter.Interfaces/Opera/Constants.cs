using OutboundAdapter.Interfaces.Models;

namespace OutboundAdapter.Interfaces.Opera
{
    public static class Constants
    {
        public static class Outbound
        {
            public static class OperaCloud
            {
                public const string UpdateRoomStatusStream = nameof(Outbound) + nameof(UpdateRoomStatus) + nameof(OperaCloud);
                public const string FetchProfileStream = nameof(Outbound) + nameof(FetchProfile) + nameof(OperaCloud);
                public const string FetchReservationStream = nameof(Outbound) + nameof(FetchReservation) + nameof(OperaCloud);
                public const string ReservationLookupStream = nameof(Outbound) + nameof(ReservationLookup) + nameof(OperaCloud);
            }
        }

        public static class Inbound
        {
            public static class V2Generic
            {
                public const string RoomStatusUpdateStream = nameof(Inbound) + nameof(RoomStatusUpdate) + nameof(V2Generic);
            }

            public static class V2
            {
                public const string RoomStatusUpdateStream = nameof(Inbound) + nameof(RoomStatusUpdate) + nameof(V2);
            }
        }
    }
}

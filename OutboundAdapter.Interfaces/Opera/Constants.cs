using OutboundAdapter.Interfaces.Models;

namespace OutboundAdapter.Interfaces.Opera
{
    public static class Constants
    {
        public const string PmsType = "Opera";
        public const string UpdateRoomStatusStream = nameof(UpdateRoomStatus) + PmsType;
        public const string FetchProfileStream = nameof(FetchProfile) + PmsType;
        public const string FetchReservationStream = nameof(FetchReservation) + PmsType;
        public const string ReservationLookupStream = nameof(ReservationLookup) + PmsType;
    }
}

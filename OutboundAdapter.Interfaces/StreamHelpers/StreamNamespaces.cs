using System.Collections.Generic;

namespace OutboundAdapter.Interfaces.StreamHelpers
{
    public class StreamNamespaces : IStreamNamespaces
    {
        public string InboundNamespace<T, P>()
        {
            return $"Inbound{typeof(T).Name}{typeof(P).Name}";
        }

        public string OutboundNamespace<T, P>()
        {
            return $"Outbound{typeof(T).Name}{typeof(P).Name}";
        }
    }

    public class HtngNamespaces : StreamNamespaces, IStreamHtngNamespaces
    {
        public const string RoomStatusUpdateBENamespace = "InboundRoomStatusUpdateBEHtng";
        public const string QueueRoomNamespace = "InboundQueueRoomHtng";
        public const string GuestStatusNotificationNamespace = "InboundGuestStatusNotificationHtng";
        public const string NewProfileNamespace = "InboundNewProfileHtng";
        public const string UpdateProfileNamespace = "InboundUpdateProfileHtng";

        public List<string> InboundNamespaces => new List<string>
            {
                RoomStatusUpdateBENamespace,
                QueueRoomNamespace,
                GuestStatusNotificationNamespace,
                NewProfileNamespace,
                UpdateProfileNamespace
            };
    }
}

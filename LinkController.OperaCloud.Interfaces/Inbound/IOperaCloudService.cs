using System.ServiceModel;
using ServiceExtensions.Soap.Oasis;

namespace LinkController.OperaCloud.Interfaces.Outbound.Inbound
{
    [ServiceContract(Namespace = "http://webservices.micros.com/htng/2008B/SingleGuestItinerary/Housekeeping/Types", ConfigurationName = "ConfigName")]
    public interface IOperaCloudService
    {
        [OperationContract(Action = "http://webservices.micros.com/htng/2008B/SingleGuestItinerary#" + nameof(Ping), ReplyAction = "*")]
        //[FaultContract(typeof(MyError), Action = "http://mynamespace.com/opName", Name = "opErr")]
        [XmlSerializerFormat()]
        [return: MessageParameter(Name = "Result")]
        OperaResponseBody Ping(string s);

        [OperationContract(Action = "http://webservices.micros.com/htng/2008B/SingleGuestItinerary#" + nameof(RoomStatusUpdateBE), ReplyAction = "*")]
        [XmlSerializerFormat()]
        [return: MessageParameter(Name = "Result")]
        OperaResponseBody RoomStatusUpdateBE(string body);

        [OperationContract(Action = "http://webservices.micros.com/htng/2008B/SingleGuestItinerary#" + nameof(QueueRoomBE), ReplyAction = "*")]
        [XmlSerializerFormat()]
        [return: MessageParameter(Name = "Result")]
        OperaResponseBody QueueRoomBE(string body);
    }

    [ServiceContract(Namespace = "http://htng.org/PWS/2008B/SingleGuestItinerary/Name/Types", ConfigurationName = "ConfigName")]
    public interface IOperaCloudServiceProfile
    {
        [OperationContract(Action = "http://htng.org/PWS/2008B/SingleGuestItinerary#" + nameof(NewProfile), ReplyAction = "*")]
        [XmlSerializerFormat()]
        [return: MessageParameter(Name = "Result")]
        OperaResponseBody NewProfile(string body);

        [OperationContract(Action = "http://htng.org/PWS/2008B/SingleGuestItinerary#" + nameof(UpdateProfile), ReplyAction = "*")]
        [XmlSerializerFormat()]
        [return: MessageParameter(Name = "Result")]
        OperaResponseBody UpdateProfile(string body);
    }

    [ServiceContract(Namespace = "http://webservices.micros.com/htng/2008B/SingleGuestItinerary/Reservation/Types", ConfigurationName = "ConfigName")]
    public interface IOperaCloudServiceGuestStatusNotification
    {
        [OperationContract(Action = "http://webservices.micros.com/htng/2008B/SingleGuestItinerary#" + nameof(GuestStatusNotificationExt), ReplyAction = "*")]
        [XmlSerializerFormat()]
        [return: MessageParameter(Name = "Result")]
        OperaResponseBody GuestStatusNotificationExt(string body);
    }

    public class MyError { }
}
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace LinkController.OperaCloud.Interfaces.Models
{
    [XmlRoot("QueueRoomBERequest", Namespace = "http://webservices.micros.com/htng/2008B/SingleGuestItinerary/Housekeeping/Types")]
    public class QueueRoomBERequestDto
    {
        [XmlAttribute("Action")]
        public ActionType Action { get; set; }

        [XmlElement("ResortId")]
        public string ResortId { get; set; }

        [XmlArray("DataElements")]
        [XmlArrayItem("DataElement", Namespace = "http://htng.org/PWS/2008B/SingleGuestItinerary/Common/Types")]
        public List<DataElementDto> DataElements { get; set; }

        [XmlIgnore]
        public string GuestNameId => DataElements.FirstOrDefault(x => x.Name == "GuestNameId").NewData ?? DataElements.FirstOrDefault(x => x.Name == "GuestNameId").OldData;

        [XmlIgnore]
        public string Name => DataElements.FirstOrDefault(x => x.Name == "Name").NewData ?? DataElements.FirstOrDefault(x => x.Name == "Name").OldData;

        [XmlIgnore]
        public string ArrivalDate => DataElements.FirstOrDefault(x => x.Name == "ArrivalDate").NewData ?? DataElements.FirstOrDefault(x => x.Name == "ArrivalDate").OldData;

        [XmlIgnore]
        public string QueueTime => DataElements.FirstOrDefault(x => x.Name == "QueueTime").NewData ?? DataElements.FirstOrDefault(x => x.Name == "QueueTime").OldData;

        [XmlIgnore]
        public string ReservationStatus => DataElements.FirstOrDefault(x => x.Name == "ReservationStatus").NewData ?? DataElements.FirstOrDefault(x => x.Name == "ReservationStatus").OldData;

        [XmlIgnore]
        public string RoomNumber => DataElements.FirstOrDefault(x => x.Name == "RoomNumber").NewData ?? DataElements.FirstOrDefault(x => x.Name == "RoomNumber").OldData;

        [XmlIgnore]
        public string RoomType => DataElements.FirstOrDefault(x => x.Name == "RoomType").NewData ?? DataElements.FirstOrDefault(x => x.Name == "RoomType").OldData;

    }
}

using System;
using System.Xml.Serialization;

namespace LinkController.OperaCloud.Interfaces.Models
{
    public class FetchRoomStatusDto
    {
        [XmlElement("RoomNumber")]
        public string RoomNumber { get; set; }

        [XmlElement("RoomType")]
        public string RoomType { get; set; }

        [XmlElement("RoomStatus")]
        public TaskRoomStatusType RoomStatus { get; set; }

        [XmlElement("RoomStatusDates")]
        public TimeSpanDto RoomStatusDates { get; set; }

        [XmlElement("FrontOfficeStatus")]
        public string FrontOfficeStatus { get; set; }

        [XmlElement("HouseKeepingStatus")]
        public string HouseKeepingStatus { get; set; }

        [XmlElement("HouseKeepingInspectionFlag")]
        public string HouseKeepingInspectionFlag { get; set; }

        [XmlElement("TurnDownYn")]
        public string TurnDownYn { get; set; }

        [XmlElement("ServiceStatus")]
        public string ServiceStatus { get; set; }

        [XmlElement("OccupancyCondition")]
        public string OccupancyCondition { get; set; }

        [XmlElement("NextReservationDate")]
        public DateTime NextReservationDate { get; set; }
    }
}
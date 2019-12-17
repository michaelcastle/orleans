using System.Collections.Generic;
using System.Xml.Serialization;

namespace LinkController.OperaCloud.Interfaces.Models
{
    public class GuestStatusDto
    {
        [XmlAttribute("reservationStatus")]
        public ReservationStatusType ReservationStatus { get; set; }

        [XmlElement("ReservationID")]
        public string ReservationId { get; set; }
        
        [XmlArray("ProfileIDs")]
        [XmlArrayItem("UniqueID", Namespace = "http://htng.org/PWS/2008B/SingleGuestItinerary/Common/Types")]
        public List<UniqueIdDto> ProfileIds { get; set; }

        [XmlElement("checkInDate")]
        public string CheckInDate { get; set; }

        [XmlElement("checkOutDate")]
        public string CheckOutDate { get; set; }

        [XmlElement("resortId")]
        public string ResortId { get; set; }
    }

    public enum ReservationStatusType
    {

        /// <remarks/>
        CANCELLED,

        /// <remarks/>
        CHECKED_IN,

        /// <remarks/>
        CHECKED_OUT,

        /// <remarks/>
        RESERVED,

        /// <remarks/>
        WAITLISTED,

        /// <remarks/>
        OTHER,

        /// <remarks/>
        REVERSE_CHECKED_IN,

        /// <remarks/>
        REVERSE_CHECKED_OUT,

        /// <remarks/>
        NO_SHOW,
    }
}

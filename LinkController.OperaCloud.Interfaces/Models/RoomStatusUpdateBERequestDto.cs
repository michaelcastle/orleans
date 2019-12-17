using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace LinkController.OperaCloud.Interfaces.Models
{
    [XmlRoot("RoomStatusUpdateBERequest", Namespace = "http://webservices.micros.com/htng/2008B/SingleGuestItinerary/Housekeeping/Types")]
    public class RoomStatusUpdateBERequestDto
    {
        [XmlElement("ResortId")]
        public string ResortId { get; set; }

        [XmlArray("DataElements")]
        [XmlArrayItem("DataElement", Namespace = "http://htng.org/PWS/2008B/SingleGuestItinerary/Common/Types")]
        public List<DataElementDto> DataElements { get; set; }

        [XmlIgnore]
        public string RoomNumber => DataElements.FirstOrDefault(x => x.Name == "RoomNumber").NewData;

        [XmlIgnore]
        public string RoomStatus => DataElements.FirstOrDefault(x => x.Name == "RoomStatus").NewData;

        [XmlIgnore]
        public string RoomType => DataElements.FirstOrDefault(x => x.Name == "RoomType").NewData;

    }
}

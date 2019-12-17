using System;
using System.Xml.Serialization;

namespace LinkController.OperaCloud.Interfaces.Models
{
    public class TimeSpanDto
    {
        [XmlElement("Start", Namespace = "http://htng.org/PWS/2008B/SingleGuestItinerary/Common/Types")]
        public DateTime Start;

        [XmlElement("End", Namespace = "http://htng.org/PWS/2008B/SingleGuestItinerary/Common/Types")]
        public DateTime End;
    }
}

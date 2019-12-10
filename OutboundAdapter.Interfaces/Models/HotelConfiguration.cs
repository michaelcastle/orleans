using System;

namespace OutboundAdapter.Interfaces.Models
{
    [Serializable]
    public class HotelConfiguration
    {
        public int HotelId { get; set; }
        public string Url { get; set; }
        public string PmsType { get; set; }
        public int Number { get; set; }
        public int TotalNumber { get; set; }
        public Credentials Credentials { get; set;}
    }

    [Serializable]
    public class Credentials
    {
        public string EncryptedUsername { get; set; }
        public string EncryptedPassword { get; set; }
    }
}

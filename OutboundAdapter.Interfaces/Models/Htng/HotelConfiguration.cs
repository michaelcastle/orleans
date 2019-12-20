using System;

namespace OutboundAdapter.Interfaces.Models
{
    [Serializable]
    public class HotelConfiguration
    {
        public int HotelId { get; set; }
        public int Number { get; set; }
        public int TotalNumber { get; set; }
        public InboundConfiguration InboundConfiguration { get; set; }
        public OutboundConfiguration OutboundConfiguration { get; set; }
    }

    [Serializable]
    public class InboundConfiguration
    {
        public string Endpoint { get; set; }
        public string Url { get; set; }
        public string InboundType { get; set; }
        public Guid ClientToken { get; set; }
        public string MediaType { get; set; }
        public Credentials Credentials { get; set; }
    }

    [Serializable]
    public class OutboundConfiguration
    {
        public string Url { get; set; }
        public string PmsType { get; set; }
        public string Endpoint { get; set; }
    }

    [Serializable]
    public class Credentials
    {
        public string EncryptedUsername { get; set; }
        public string EncryptedPassword { get; set; }
    }

    public static class InboundConfigurationExtensions
    {
        public static string Key(this InboundConfiguration inboundConfiguration)
        {
            return $"{inboundConfiguration.InboundType}{inboundConfiguration.Url}";
        }
    }
}

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
        public PmsConfiguration OutboundConfiguration { get; set; }
    }

    [Serializable]
    public class InboundConfiguration : SubscribeEndpoint
    {
        public Guid ClientToken { get; set; }
        public string MediaType { get; set; }
    }

    [Serializable]
    public class PmsConfiguration : SubscribeEndpoint
    {
    }

    [Serializable]
    public class SubscribeEndpoint : ISubscribeEndpoint
    {
        public string Url { get; set; }
        public string Endpoint { get; set; }
        public Credentials Credentials { get; set; }
    }

    [Serializable]
    public class Credentials
    {
        public string EncryptedUsername { get; set; }
        public string EncryptedPassword { get; set; }
    }

    public interface ISubscribeEndpoint
    {
        string Url { get; set; }
        string Endpoint { get; set; }
        Credentials Credentials { get; set; }
    }

    public static class Extensions
    {
        public static string CompoundKeyEndpoint(this ISubscribeEndpoint configuration)
        {
            var baseUrl = new Uri(configuration.Url);
            var endpointUrl = new Uri(baseUrl, configuration.Endpoint);
            return endpointUrl.AbsoluteUri;
        }
    }
}

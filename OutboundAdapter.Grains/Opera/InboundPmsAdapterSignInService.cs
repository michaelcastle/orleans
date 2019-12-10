using Orleans;
using OutboundAdapter.Interfaces;
using ServiceExtensions.PmsAdapter.SignIn;

namespace OutboundAdapter.Grains.Opera
{
    public class InboundPmsAdapterSignInService : ISessionItemAuthenticationService
    {
        private readonly IClusterClient _clusterClient;

        public InboundPmsAdapterSignInService(IClusterClient clusterClient)
        {
            _clusterClient = clusterClient;
        }

        public SessionItem SignIn(string username, string password, string lastAction, string hotelId)
        {
            int.TryParse(hotelId, out int hotelIdInt);
            var hotel = _clusterClient.GetGrain<IHotelPmsGrain>(hotelIdInt);
            if (!hotel.IsConnected().Result)
            {
                return null;
            }

            var config = hotel.GetOutboundConfiguration().Result;
            if (config.Credentials.EncryptedUsername == username && config.Credentials.EncryptedPassword == password)
            {
                return new SessionItem()
                {
                    SessionId = new System.Guid(),
                    UserName = username,
                    IsAuthorised = true
                };
            }

            return null;
        }
    }
}

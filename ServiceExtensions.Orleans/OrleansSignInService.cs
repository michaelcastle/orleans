using Orleans;
using OutboundAdapter.Interfaces;
using ServiceExtensions.PmsAdapter.SignIn;
using System;

namespace ServiceExtensions.Orleans
{
    public class OrleansSignInService : ISessionItemAuthenticationService
    {
        private readonly IClusterClient _clusterClient;

        public OrleansSignInService(IClusterClient clusterClient)
        {
            _clusterClient = clusterClient;
        }

        public SessionItem SignIn(string username, string password, string lastAction, string hotelId)
        {
            //return new SessionItem()
            //{
            //    SessionId = Guid.NewGuid(),
            //    UserName = username,
            //    IsAuthorised = true
            //};

            int.TryParse(hotelId, out int hotelIdInt);
            var hotel = _clusterClient.GetGrain<IHotelPmsGrain>(hotelIdInt);
            if (!hotel.IsInboundConnected().Result)
            {
                return null;
            }

            var config = hotel.GetInboundConfiguration().Result;
            if (config.Credentials.EncryptedUsername == username && config.Credentials.EncryptedPassword == password)
            {
                return new SessionItem()
                {
                    SessionId = Guid.NewGuid(),
                    UserName = username,
                    IsAuthorised = true
                };
            }

            return null;
        }
    }
}

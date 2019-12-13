using System;
using System.Security.Authentication;
using System.ServiceModel.Channels;
using Microsoft.AspNetCore.Http;
using Orleans;
using OutboundAdapter.Interfaces.Opera.Inbound;
using ServiceExtensions.Soap.Core;
using ServiceExtensions.Soap.Core.Oasis;

namespace OutboundAdapter.Grains.Opera
{
    public class LinkControllerOasisMessageFilter : IMessageFilter
    {
        private const string AuthMissingErrorMessage = "Referenced security token could not be retrieved";
        public readonly string AuthTimestampMessage = "Invalid Timestamp - timestamp was not valid";
        public readonly string AuthInvalidErrorMessage = "Authentication error: Authentication failed: the supplied credential are not valid";

        public OasisSecurity OasisSecurity;
        private readonly IClusterClient _clusterClient;

        private readonly IOasisSecurityService _oasisSecurityService;

        public LinkControllerOasisMessageFilter(IOasisSecurityService oasisSecurityService, IClusterClient clusterClient)
        {
            _clusterClient = clusterClient;
            _oasisSecurityService = oasisSecurityService;
        }

        public void OnRequestExecuting(Message message, PathString path)
        {
            OasisUsernameToken oasisUsernameToken;

            try
            {
                OasisSecurity = _oasisSecurityService.GetOasisSecurity(message.Headers);
                oasisUsernameToken = OasisSecurity.UsernameToken;
            }
            catch (Exception ex)
            {
                var errmessage = ex.Message;
                throw new AuthenticationException(AuthMissingErrorMessage);
            }

            if (!ValidateOasisTimestamp(OasisSecurity))
            {
                throw new AuthenticationException($"{AuthTimestampMessage}. Timestamp: {OasisSecurity.Timestamp.Created}, {OasisSecurity.Timestamp.Expires}");
            }

            try
            {
                if (!ValidateOasisUsernameToken(oasisUsernameToken, path))
                {
                    throw new InvalidCredentialException($"{AuthInvalidErrorMessage}. Username: {oasisUsernameToken.Username}");
                }
            }
            catch (InvalidCredentialException)
            {
                throw new InvalidCredentialException($"{AuthInvalidErrorMessage}. Username: {oasisUsernameToken.Username}");
            }
        }

        public void OnResponseExecuting(Message message)
        {
            //empty
        }

        private static bool ValidateOasisTimestamp(OasisSecurity oasisSecurity)
        {
            return (DateTimeOffset.Now > oasisSecurity.Timestamp.Created && DateTimeOffset.Now < oasisSecurity.Timestamp.Expires);
        }

        private bool ValidateOasisUsernameToken(OasisUsernameToken oasisUsernameToken, PathString path)
        {
            if (string.IsNullOrEmpty(oasisUsernameToken.Username) && !string.IsNullOrEmpty(oasisUsernameToken.Password))
            {
                return false;
            }

            var queryString = new Uri(path.Value).Query;
            var queryDictionary = System.Web.HttpUtility.ParseQueryString(queryString);
            var hotelId = queryDictionary.Get("hotelid");
            if (string.IsNullOrEmpty(hotelId))
            {
                return false;
            }
            int.TryParse(hotelId, out int hotelIdInt);
            var authenticatorGrain = _clusterClient.GetGrain<IAuthenticateOracleCloud>(hotelIdInt);

            return authenticatorGrain.Validate(oasisUsernameToken.Username, oasisUsernameToken.Password).Result;
        }
    }
}

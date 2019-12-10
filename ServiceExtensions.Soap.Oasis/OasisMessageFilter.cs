using System;
using System.Security.Authentication;
using System.ServiceModel.Channels;
using ServiceExtensions.PmsAdapter.SignIn.Authentication;
using ServiceExtensions.Soap.Core;
using ServiceExtensions.Soap.Core.Oasis;

namespace ServiceExtensions.Soap.Oasis
{
    public class OasisMessageFilter : IMessageFilter 
    {
        private const string AuthMissingErrorMessage = "Referenced security token could not be retrieved";
        public readonly string AuthTimestampMessage = "Invalid Timestamp - timestamp was not valid";
        public readonly string AuthInvalidErrorMessage = "Authentication error: Authentication failed: the supplied credential are not valid";

        public OasisSecurity OasisSecurity;

        private readonly ISecurityAuthenticator _authenticator;
        private readonly IOasisSecurityService _oasisSecurityService;

        public OasisMessageFilter(ISecurityAuthenticator authenticator, IOasisSecurityService oasisSecurityService)
        {
            _authenticator = authenticator;
            _oasisSecurityService = oasisSecurityService;
        }

        public void OnRequestExecuting(Message message)
        {
            OasisUsernameToken oasisUsernameToken = null;

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
                if (!ValidateOasisUsernameToken(oasisUsernameToken))
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

        private bool ValidateOasisUsernameToken(OasisUsernameToken oasisUsernameToken)
        {
            return _authenticator.Validate(oasisUsernameToken.Username, oasisUsernameToken.Password, "1");
        }
    }
}

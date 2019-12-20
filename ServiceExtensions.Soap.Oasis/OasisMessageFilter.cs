using Microsoft.AspNetCore.Http;
using ServiceExtensions.PmsAdapter.ClientChannel;
using ServiceExtensions.PmsAdapter.Connected_Services.PmsProcessor;
using ServiceExtensions.PmsAdapter.SignIn.Authentication;
using ServiceExtensions.Soap.Core;
using ServiceExtensions.Soap.Core.Oasis;
using System;
using System.Security.Authentication;
using System.ServiceModel.Channels;

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
        private readonly IClientChannelFactory<IPMSInterfaceContractChannel> _clientFactory;

        public OasisMessageFilter(ISecurityAuthenticator authenticator, IOasisSecurityService oasisSecurityService, IClientChannelFactory<IPMSInterfaceContractChannel> clientFactory)
        {
            _authenticator = authenticator;
            _oasisSecurityService = oasisSecurityService;
            _clientFactory = clientFactory;
        }

        public void OnRequestExecuting(Message message, HttpRequest path)
        {
            OasisUsernameToken oasisUsernameToken;

            try
            {
                OasisSecurity = _oasisSecurityService.GetOasisSecurity(message.Headers);
                oasisUsernameToken = OasisSecurity.UsernameToken;
            }
            catch (Exception ex)
            {
                _ = ex.Message;
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
            return _authenticator.Validate(_clientFactory, oasisUsernameToken.Username, oasisUsernameToken.Password).Result;
        }
    }
}

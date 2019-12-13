using Orleans;
using ServiceExtensions.PmsAdapter.ClientChannel;
using ServiceExtensions.PmsAdapter.Connected_Services.PmsProcessor;
using ServiceExtensions.PmsAdapter.SignIn;

namespace ServiceExtensions.Orleans
{
    public class ClientFactorySignInService : ISessionItemAuthenticationService
    {

        public SessionItem SignIn(IClientChannelFactory<IPMSInterfaceContractChannel> _clientFactory, string username, string password)
        {
            var client = _clientFactory.CreateChannel();
            try
            {
                var signinRequest = new SigninRequest(username, password);
                var signin = client.SigninAsync(signinRequest).Result;

                if (signin.InterfaceSigninValues == null)
                {
                    return null;
                }

                return new SessionItem()
                {
                    SessionId = signin.InterfaceSigninValues.SessionId,
                    UserName = username,
                    IsAuthorised = true
                };
            }
            finally
            {
                _clientFactory.CloseChannel(client);
            }
        }
    }
}

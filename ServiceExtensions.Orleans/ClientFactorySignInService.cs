using ServiceExtensions.PmsAdapter.ClientChannel;
using ServiceExtensions.PmsAdapter.Connected_Services.PmsProcessor;
using ServiceExtensions.PmsAdapter.SignIn;
using System;
using System.Threading.Tasks;

namespace ServiceExtensions.Orleans
{
    public class ClientFactorySignInService : ISessionItemAuthenticationService
    {
        public async Task<SessionItem> SignIn(IClientChannelFactory<IPMSInterfaceContractChannel> _clientFactory, string username, string password)
        {
            var client = _clientFactory.CreateChannel();
            try
            {
                var signinRequest = new SigninRequest(username, password);
                var signin = await client.SigninAsync(signinRequest);

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
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _clientFactory.CloseChannel(client);
            }
        }
    }
}

using Microsoft.Extensions.Options;
using ServiceExtensions.PmsAdapter.ClientChannel;
using ServiceExtensions.PmsAdapter.Connected_Services.PmsProcessor;
using ServiceExtensions.PmsAdapter.Encryption;

namespace ServiceExtensions.PmsAdapter.SignIn
{
    public class PmsProcessorSignInService : ISessionItemAuthenticationService
    {
        private readonly IClientChannelFactory<IPMSInterfaceContractChannel> _clientFactory;
        private readonly EncryptionSettings _encryptionSettings;

        public PmsProcessorSignInService(IClientChannelFactory<IPMSInterfaceContractChannel> clientFactory, IOptions<EncryptionSettings> encryptionSettings)
        {
            _clientFactory = clientFactory;
            _encryptionSettings = encryptionSettings.Value;
        }

        public SessionItem SignIn(string username, string password, string lastAction, string hotelId)
        {
            var client = _clientFactory.CreateChannel();
            try
            {
                if (_encryptionSettings.EncryptCredentials)
                {
                    var encryptionService = new OptiiEncryptionService();
                    username = encryptionService.Encrypt(username, _encryptionSettings.UsernameSharedSecret, _encryptionSettings.Salt);
                    password = encryptionService.Encrypt(password, _encryptionSettings.PasswordSharedSecret, _encryptionSettings.Salt);
                }

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

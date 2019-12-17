using Orleans;
using Orleans.Runtime;
using OutboundAdapter.Interfaces.Models;
using System.Threading.Tasks;

namespace LinkController.OperaCloud.OperaCloud
{
    public class AuthenticateOracleCloud : Grain, AuthenticateOracleCloud
    {
        private readonly IPersistentState<Credentials> _credentials;

        public AuthenticateOracleCloud([PersistentState("oracleCloudCredentials", "oracleCloudStore")] IPersistentState<Credentials> credentials)
        {
            _credentials = credentials;
        }

        async Task IAuthenticateOracleCloud.DeleteCredentials()
        {
            await _credentials.ClearStateAsync();
        }

        async Task IAuthenticateOracleCloud.SetCredentials(Credentials credentials)
        {
            _credentials.State = credentials;
            await _credentials.WriteStateAsync();
        }

        Task<bool> IAuthenticateOracleCloud.Validate(string userName, string password)
        {
            return Task.FromResult(userName == _credentials.State.EncryptedUsername && password == _credentials.State.EncryptedPassword);
        }
    }
}

using LinkController.OperaCloud.Interfaces.Outbound.Inbound;
using Orleans;
using Orleans.Runtime;
using OutboundAdapter.Interfaces.Models;
using System.Threading.Tasks;

namespace LinkController.OperaCloud.Grains
{
    public class AuthenticateOracleCloudGrains : Grain, IAuthenticateOracleCloudGrains
    {
        private readonly IPersistentState<Credentials> _credentials;

        public AuthenticateOracleCloudGrains([PersistentState("oracleCloudCredentials", "oracleCloudStore")] IPersistentState<Credentials> credentials)
        {
            _credentials = credentials;
        }

        async Task IAuthenticateOracleCloudGrains.DeleteCredentials()
        {
            await _credentials.ClearStateAsync();
        }

        async Task IAuthenticateOracleCloudGrains.SetCredentials(Credentials credentials)
        {
            _credentials.State = credentials;
            await _credentials.WriteStateAsync();
        }

        Task<bool> IAuthenticateOracleCloudGrains.Validate(string userName, string password)
        {
            return Task.FromResult(userName == _credentials.State.EncryptedUsername && password == _credentials.State.EncryptedPassword);
        }
    }
}

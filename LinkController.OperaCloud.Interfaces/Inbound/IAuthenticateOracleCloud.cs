using OutboundAdapter.Interfaces.Models;
using System.Threading.Tasks;

namespace LinkController.OperaCloud.Interfaces.Outbound.Inbound
{
    public interface IAuthenticateOracleCloud : Orleans.IGrainWithIntegerKey
    {
        Task SetCredentials(Credentials credentials);
        Task DeleteCredentials();
        Task<bool> Validate(string userName, string password);
    }
}

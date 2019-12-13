using OutboundAdapter.Interfaces.Models;
using System.Threading.Tasks;

namespace OutboundAdapter.Interfaces.Opera.Inbound
{
    public interface IAuthenticateOracleCloud : Orleans.IGrainWithIntegerKey
    {
        Task SetCredentials(Credentials credentials);
        Task DeleteCredentials();
        Task<bool> Validate(string userName, string password);
    }
}

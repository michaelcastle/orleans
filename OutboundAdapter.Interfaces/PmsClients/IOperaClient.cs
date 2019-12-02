using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OutboundAdapter.Interfaces.PmsClients
{
    public interface IOperaHTNG2008BServiceClient
    {
        Task<string> HTNG2008BService(string request);
    }

    public interface IOperaHTNG_EXT2008BWebServicesClient
    {
        Task<string> OperaHTNG_EXT2008BWebServices(string request);
    }
}

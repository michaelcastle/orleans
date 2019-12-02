using Orleans;
using Orleans.Concurrency;
using OutboundAdapter.Interfaces;
using OutboundAdapter.Interfaces.PmsClients;
using System.Threading.Tasks;

namespace OutboundAdapter.Grains
{
    [StatelessWorker]
    public class HttpFactoryOperaGrain : Grain, IHttpFactoryGrain
    {
        private readonly IOperaHTNG2008BServiceClient _htng2008bClient;
        private readonly IOperaHTNG_EXT2008BWebServicesClient _htng2008BExtClient;

        public HttpFactoryOperaGrain(IOperaHTNG2008BServiceClient htng2008bClient, IOperaHTNG_EXT2008BWebServicesClient htng2008BExtClient)
        {
            _htng2008bClient = htng2008bClient;
            _htng2008BExtClient = htng2008BExtClient;
        }

        async Task<string> IHttpFactoryGrain.FetchProfile(string content)
        {
            return await _htng2008bClient.HTNG2008BService(content);
        }

        async Task<string> IHttpFactoryGrain.FetchReservation(string content)
        {
            // return await _operaClient.OperaHTNG_EXT2008BWebServices(request); // Opera Cloud
            return await _htng2008bClient.HTNG2008BService(content);
        }

        async Task<string> IHttpFactoryGrain.ReservationLookup(string content)
        {
            return await _htng2008bClient.HTNG2008BService(content);
        }

        Task<string> IHttpFactoryGrain.UpdateRoomStatus(string content)
        {
            return _htng2008BExtClient.OperaHTNG_EXT2008BWebServices(content);
        }
    }
}

using OutboundAdapter.Interfaces.Models;
using System.Threading.Tasks;

namespace OutboundAdapter.Interfaces
{
    public interface IHotelPmsGrain : Orleans.IGrainWithIntegerKey
    {
        Task<HotelConfiguration> Get();
        Task<IOutboundMappingGrains> GetOutboundMapper();
        //Task<IInboundAdapterGrain> GetInboundMapper();
        Task<IHttpFactoryGrain> GetClient();
        Task IncrementAsync();
    }
}

using LinkController.OperaCloud.Interfaces;
using LinkController.OperaCloud.Interfaces.Models;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using Orleans.Streams;
using OutboundAdapter.Interfaces;
using System.Threading.Tasks;

namespace PmsAdapter.Api.OperaCloud.Outbound.Controllers
{
    [Route("api/[controller]/opera")]
    [ApiController]
    public class OutboundController : ControllerBase
    {
        private readonly IClusterClient _clusterClient;
        private const string StreamProviderName = "SMSProvider";
        private readonly IStreamProvider _streamProvider;

        public OutboundController(IClusterClient clusterClient)
        {
            _clusterClient = clusterClient;
            _streamProvider = clusterClient.GetStreamProvider(StreamProviderName);
        }

        [HttpPost("UpdateRoomStatus/{hotelId}")]
        public async Task<IActionResult> UpdateRoomStatus(int hotelId, [FromBody]UpdateRoomStatusRequestDto content)
        {
            var hotel = _clusterClient.GetGrain<IHotelPmsGrain>(hotelId);
            var stream = _streamProvider.GetStream<UpdateRoomStatusRequestDto>(hotel.GetPrimaryKey(), Constants.Outbound.OperaCloud.UpdateRoomStatusRequestStream);
            await stream.OnNextAsync(content);

            return Ok();
        }
    }
}
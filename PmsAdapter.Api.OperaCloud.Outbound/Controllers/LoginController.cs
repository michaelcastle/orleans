using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using OutboundAdapter.Interfaces;
using OutboundAdapter.Interfaces.Models;
using OutboundAdapter.Interfaces.Opera;

namespace PmsAdapter.Api.OperaCloud.Outbound.Controllers
{
    [Route("api/[controller]/OperaCloud")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IClusterClient _clusterClient;

        public LoginController(IClusterClient clusterClient)
        {
            _clusterClient = clusterClient;
        }

        [HttpPost("Connect/{hotelId}")]
        public async Task<IActionResult> Connect(int hotelId, [FromBody]OutboundConfiguration configuration)
        {
            configuration.PmsType = nameof(Constants.Outbound.OperaCloud);

            var hotel = _clusterClient.GetGrain<IHotelPmsGrain>(hotelId);
            var task = hotel.SaveOutboundConfigurationAsync(configuration);
            await task;
            if (!task.IsCompletedSuccessfully)
            {
                return StatusCode(500);
            }
            return Ok();
        }

        public async Task<IActionResult> SubscribeFrom(int hotelId)
        {
            return Ok();
        }

        [HttpPost("Subscribe/{hotelId}")]
        public async Task<IActionResult> Subscribe(int hotelId, [FromBody]InboundConfiguration configuration)
        {
            var hotel = _clusterClient.GetGrain<IHotelPmsGrain>(hotelId);
            var task = hotel.SubscribeTo(configuration);
            await task;
            if (!task.IsCompletedSuccessfully)
            {
                return StatusCode(500);
            }
            return Ok();
        }
    }
}

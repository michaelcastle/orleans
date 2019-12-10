using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using OutboundAdapter.Interfaces;
using OutboundAdapter.Interfaces.Models;
using OutboundAdapter.Interfaces.Opera;

namespace PmsAdapter.Api.Controllers.Opera
{
    [Route("api/[controller]/opera")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IClusterClient _clusterClient;

        public LoginController(IClusterClient clusterClient)
        {
            _clusterClient = clusterClient;
        }

        [HttpPost("Connect/{hotelId}")]
        public async Task<IActionResult> Connect(int hotelId, [FromBody]HotelConfiguration configuration)
        {
            configuration.PmsType = Constants.PmsType;
            var hotel = _clusterClient.GetGrain<IHotelPmsGrain>(hotelId);
            var task = hotel.SaveOutboundConfigurationAsync(configuration);
            await task;
            if (!task.IsCompletedSuccessfully)
            {
                return StatusCode(500);
            }
            return Ok();
        }
    }
}

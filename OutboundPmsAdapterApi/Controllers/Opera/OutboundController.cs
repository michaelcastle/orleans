using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using OutboundAdapter.Interfaces;
using OutboundAdapter.Interfaces.Models;
using OutboundAdapter.Interfaces.Opera;
using OutboundAdapter.Interfaces.Opera.Models;

namespace OutboundPmsAdapterApi.Controllers.Opera
{
    [Route("api/[controller]/opera")]
    [ApiController]
    public class OutboundController : ControllerBase
    {
        private readonly IClusterClient _clusterClient;
        private static int counter = 0;

        public OutboundController(IClusterClient clusterClient)
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

        [HttpPost("UpdateRoomStatus/{hotelId}")]
        public async Task<IActionResult> UpdateRoomStatus(int hotelId, [FromBody]UpdateRoomStatusRequestDto content)
        {
            var hotel = _clusterClient.GetGrain<IHotelPmsGrain>(hotelId);
            if (!await hotel.IsConnected())
            {
                return BadRequest("Hotel is not connected to the PMS. Please Connect first.");
            }

            var mapper = _clusterClient.GetGrain<IOutboundMappingGrains>(hotelId);
            var request = mapper.MapUpdateRoomStatus(content);

            var hotelGrain = _clusterClient.GetGrain<IOutboundAdapterGrain>(hotelId);
            var response = await hotelGrain.UpdateRoomStatus(counter++, new UpdateRoomStatus
            {
                Request = await request
            });

            return Ok(response);
        }
    }
}
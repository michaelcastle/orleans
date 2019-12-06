using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using OutboundAdapter.Interfaces;
using OutboundAdapter.Interfaces.Models;

namespace OutboundPmsAdapterApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OutboundController : ControllerBase
    {
        private readonly IClusterClient _clusterClient;
        private static int counter = 0;

        public OutboundController(IClusterClient clusterClient)
        {
            _clusterClient = clusterClient;
        }

        [HttpPost("{hotelId}/Connect")]
        public async Task<IActionResult> Connect(int hotelId, [FromBody]HotelConfiguration configuration)
        {
            var hotel = _clusterClient.GetGrain<IHotelPmsGrain>(hotelId);
            var task = hotel.SaveOutboundConfigurationAsync(configuration);
            await task;
            if (!task.IsCompletedSuccessfully)
            {
                return StatusCode(500);
            }
            return Ok();
        }

        [HttpPost("{hotelId}/UpdateRoomStatus")]
        public async Task<IActionResult> UpdateRoomStatus(int hotelId, [FromBody]string content)
        {
            var hotel = _clusterClient.GetGrain<IHotelPmsGrain>(hotelId);
            if (!await hotel.IsConnected())
            {
                return BadRequest("Hotel is not connected to the PMS. Please Connect first.");
            }

            var hotelGrain = _clusterClient.GetGrain<IOutboundAdapterGrain>(hotelId);
            var response = await hotelGrain.UpdateRoomStatus(counter++, content); 
            return Ok(response);
        }
    }
}
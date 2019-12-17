﻿using System.Threading.Tasks;
using LinkController.OperaCloud.Interfaces.Models;
using LinkController.OperaCloud.Interfaces.Outbound;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using OutboundAdapter.Interfaces;
using OutboundAdapter.Interfaces.Models;

namespace PmsAdapter.Api.OperaCloud.Outbound.Controllers
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

        [HttpPost("UpdateRoomStatus/{hotelId}")]
        public async Task<IActionResult> UpdateRoomStatus(int hotelId, [FromBody]UpdateRoomStatusRequestDto content)
        {
            var hotel = _clusterClient.GetGrain<IHotelPmsGrain>(hotelId);
            if (!await hotel.IsOutboundConnected())
            {
                return BadRequest("Hotel is not connected to the PMS. Please Connect first.");
            }

            var mapper = _clusterClient.GetGrain<IOutboundMappingOperaGrains>(hotelId);
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
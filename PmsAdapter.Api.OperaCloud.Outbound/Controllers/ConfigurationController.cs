using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using OutboundAdapter.Interfaces;
using OutboundAdapter.Interfaces.Consumer;
using OutboundAdapter.Interfaces.Models;
using OutboundAdapter.Interfaces.Opera;
using OutboundAdapter.Interfaces.Opera.Inbound;

namespace PmsAdapter.Api.OperaCloud.Outbound.Controllers
{
    [Route("api/[controller]/OperaCloud")]
    [ApiController]
    public class ConfigurationController : ControllerBase
    {
        private readonly IClusterClient _clusterClient;
        private const string StreamProviderName = "SMSProvider";

        public ConfigurationController(IClusterClient clusterClient)
        {
            _clusterClient = clusterClient;
        }

        [HttpPost("Publish/{hotelId}")]
        public async Task<IActionResult> Publish(int hotelId, [FromBody]OutboundConfiguration configuration)
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

        [HttpPost("AuthenticateSubscribers/{hotelId}")]
        public async Task<IActionResult> AuthenticateSubscribers(int hotelId, [FromBody]Credentials credentials)
        {
            var credentialsGrain = _clusterClient.GetGrain<IAuthenticateOracleCloud>(hotelId);
            await credentialsGrain.SetCredentials(credentials);
            return Ok();
        }

        [HttpPost("Subscribe/{hotelId}")]
        public async Task<IActionResult> Subscribe(int hotelId, [FromBody]InboundConfiguration configuration)
        {
            var hotel = _clusterClient.GetGrain<IHotelPmsGrain>(hotelId);

            var namespaces = new List<string>
            {
                await hotel.StreamNamespaceInbound<RoomStatusUpdate, Constants.Outbound.OperaCloud>(),
                await hotel.StreamNamespaceOutbound<UpdateRoomStatusResponse>()
                //await hotel.StreamNamespaceInbound<FetchProfileResponse>(nameof(Constants.Outbound.OperaCloud))
                //await hotel.StreamNamespaceInbound<FetchReservationResponse>(nameof(Constants.Outbound.OperaCloud))
                //await hotel.StreamNamespaceInbound<ReservationLookupResponse>(nameof(Constants.Outbound.OperaCloud))
            };

            var consumerGrain = _clusterClient.GetGrain<IInboundConsumerGrain>(0);
            var observer = await consumerGrain.GetInboundConsumer(configuration, hotelId);
            var task = hotel.SubscribeToInbound(configuration, StreamProviderName, namespaces, observer);

            await task;
            if (!task.IsCompletedSuccessfully)
            {
                return StatusCode(500);
            }

            return Ok();
        }

        [HttpPost("Unsubscribe/{hotelId}")]
        public async Task<IActionResult> Unsubscribe(int hotelId, [FromBody]InboundConfiguration configuration)
        {
            var hotel = _clusterClient.GetGrain<IHotelPmsGrain>(hotelId);

            await hotel.Unsubscribe(configuration);

            return Ok();
        }
    }
}

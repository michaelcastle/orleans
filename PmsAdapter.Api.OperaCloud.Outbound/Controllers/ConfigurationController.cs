using LinkController.OperaCloud.Interfaces;
using LinkController.OperaCloud.Interfaces.Models;
using LinkController.OperaCloud.Interfaces.Outbound.Inbound;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using OutboundAdapter.Interfaces;
using OutboundAdapter.Interfaces.Consumer;
using OutboundAdapter.Interfaces.Models;
using OutboundAdapter.Interfaces.StreamHelpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PmsAdapter.Api.OperaCloud.Outbound.Controllers
{
    [Route("api/[controller]/OperaCloud")]
    [ApiController]
    public class ConfigurationController : ControllerBase
    {
        private readonly IClusterClient _clusterClient;
        private const string StreamProviderName = "SMSProvider";
        private readonly IStreamNamespaces _streamNamspaces;

        public ConfigurationController(IClusterClient clusterClient, IStreamNamespaces streamNamspaces)
        {
            _clusterClient = clusterClient;
            _streamNamspaces = streamNamspaces;
        }

        [HttpPost("Connect/{hotelId}")]
        public async Task<IActionResult> Connect(int hotelId, [FromBody]OutboundConfiguration configuration)
        {
            configuration.PmsType = nameof(Constants.Outbound.OperaCloud);

            var hotel = _clusterClient.GetGrain<IHotelPmsGrain>(hotelId);
            var task = hotel.SaveConsumerConfigurationAsync(configuration);
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
            var credentialsGrain = _clusterClient.GetGrain<IAuthenticateOracleCloudGrains>(hotelId);
            await credentialsGrain.SetCredentials(credentials);
            return Ok();
        }

        [HttpPost("Subscribe/{hotelId}")]
        public async Task<IActionResult> Subscribe(int hotelId, [FromBody]InboundConfiguration configuration)
        {
            var hotel = _clusterClient.GetGrain<IHotelPmsGrain>(hotelId);

            var namespaces = new List<string>
            {
                _streamNamspaces.InboundNamespace<RoomStatusUpdateBERequestDto, Constants.Outbound.OperaCloud>(),
                _streamNamspaces.InboundNamespace<GuestStatusNotificationExtRequestDto, Constants.Outbound.OperaCloud>(),
                _streamNamspaces.InboundNamespace<QueueRoomBERequestDto, Constants.Outbound.OperaCloud>(),
                _streamNamspaces.InboundNamespace<NewProfileRequestDto, Constants.Outbound.OperaCloud>(),
                _streamNamspaces.InboundNamespace<UpdateProfileRequestDto, Constants.Outbound.OperaCloud>(),
                _streamNamspaces.OutboundNamespace<UpdateRoomStatusResponseEnvelopeDto, Constants.Outbound.OperaCloud>()
                //_streamNamspaces.OutboundNamespace<FetchProfileResponse, Constants.Outbound.OperaCloud>()
                //_streamNamspaces.OutboundNamespace<FetchReservationResponse, Constants.Outbound.OperaCloud>()
                //_streamNamspaces.OutboundNamespace<ReservationLookupResponse, Constants.Outbound.OperaCloud>()
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

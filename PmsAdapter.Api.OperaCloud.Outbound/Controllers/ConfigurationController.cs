using LinkController.OperaCloud.Interfaces.Outbound;
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
        private readonly IStreamV2Namespaces _streamV2Namespaces;

        public ConfigurationController(IClusterClient clusterClient, IStreamV2Namespaces streamV2Namespaces)
        {
            _clusterClient = clusterClient;
            _streamV2Namespaces = streamV2Namespaces;
        }

        [HttpPost("ConnectPms/{hotelId}")]
        public async Task<IActionResult> ConnectPms(int hotelId, [FromBody]PmsConfiguration configuration)
        {
            var hotel = _clusterClient.GetGrain<IHotelPmsGrain>(hotelId);
            //await hotel.SavePmsConfigurationAsync(configuration);

            await hotel.Subscribe<IUpdateRoomStatusRequestOperaCloudConsumer>(StreamProviderName, configuration);

            //var namespaces = new List<string> () { Constants.Outbound.OperaCloud.UpdateRoomStatusRequestStream);
            //var observer = _clusterClient.GetGrain<IUpdateRoomStatusRequestOperaCloudConsumer>((int)hotel.GetPrimaryKeyLong(), configuration.CompoundKeyEndpoint());
            //await observer.SetConfiguration(configuration);
            //await hotel.SubscribeResponses(StreamProviderName, namespaces, observer);

            return Ok();
        }

        [HttpPost("AuthenticatePms/{hotelId}")]
        public async Task<IActionResult> AuthenticatePms(int hotelId, [FromBody]Credentials credentials)
        {
            var credentialsGrain = _clusterClient.GetGrain<IAuthenticateOracleCloudGrains>(hotelId);
            await credentialsGrain.SetCredentials(credentials);
            return Ok();
        }

        [HttpPost("Subscribe/{hotelId}")]
        public async Task<IActionResult> Subscribe(int hotelId, [FromBody]InboundConfiguration configuration)
        {
            var hotel = _clusterClient.GetGrain<IHotelPmsGrain>(hotelId);

            var observer = _clusterClient.GetGrain<ISubmitMessageConsumer>((int)hotel.GetPrimaryKeyLong(), configuration.CompoundKeyEndpoint());
            await observer.SetConfiguration(configuration);
            await hotel.SubscribeResponses(StreamProviderName, _streamV2Namespaces.InboundNamespaces, observer);

            return Ok();
        }

        [HttpPost("SubscribeHtng/{hotelId}")]
        public async Task<IActionResult> SubscribeHtng(int hotelId, [FromBody]InboundConfiguration configuration)
        {
            var hotel = _clusterClient.GetGrain<IHotelPmsGrain>(hotelId);

            var tasks = new List<Task>
            {   
                //hotel.SubscribeHtng<IQueueRoomConsumer>(StreamProviderName, configuration),
                //hotel.SubscribeHtng<IUpdateProfileConsumer>(StreamProviderName, configuration),
                //hotel.SubscribeHtng<INewProfileConsumer>(StreamProviderName, configuration),
                //hotel.SubscribeHtng<IGuestStatusNotificationConsumer>(StreamProviderName, configuration),
                hotel.Subscribe<IRoomStatusUpdateBEConsumer>(StreamProviderName, configuration)
            };

            await Task.WhenAll(tasks.ToArray());

            return Ok();
        }

        [HttpPost("Unsubscribe/{hotelId}")]
        public async Task<IActionResult> Unsubscribe(int hotelId, [FromBody]InboundConfiguration configuration)
        {
            var observer = _clusterClient.GetGrain<ISubmitMessageConsumer>(hotelId, configuration.CompoundKeyEndpoint());
            await observer.StopConsuming();

            return Ok();
        }

        [HttpPost("UnsubscribeHtng/{hotelId}")]
        public async Task<IActionResult> UnsubscribeHtng(int hotelId, [FromBody]InboundConfiguration configuration)
        {
            var tasks = new List<Task>();
            var hotel = _clusterClient.GetGrain<IHotelPmsGrain>(hotelId);

            //tasks.Add(hotel.UnsubscribeHtng<IQueueRoomConsumer>(configuration));
            //tasks.Add(hotel.UnsubscribeHtng<IUpdateProfileConsumer>(configuration));
            //tasks.Add(hotel.UnsubscribeHtng<INewProfileConsumer>(configuration));
            //tasks.Add(hotel.UnsubscribeHtng<IGuestStatusNotificationConsumer>(configuration));
            tasks.Add(hotel.UnsubscribeHtng<IRoomStatusUpdateBEConsumer>(configuration));

            await Task.WhenAll(tasks.ToArray());

            return Ok();
        }
    }
}

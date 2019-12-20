using LinkController.OperaCloud.Interfaces;
using LinkController.OperaCloud.Interfaces.Outbound.Inbound;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Orleans;
using ServiceExtensions.Soap.Oasis;
using System;
using System.Linq;

namespace PmsAdapter.Api.OperaCloud.Inbound.Controllers
{
    public class InboundController : IOperaCloudService, IOperaCloudServiceProfile, IOperaCloudServiceGuestStatusNotification
    {
        private readonly ILogger<InboundController> _logger;
        private readonly IClusterClient _clusterClient;

        // TODO get this somehow?
        private string HotelId => Query["hotelid"].FirstOrDefault();

        public System.ServiceModel.Channels.MessageHeaders MessageHeaders { get; set; }
        public QueryCollection Query { get; set; }

        public InboundController(ILogger<InboundController> logger, IClusterClient clusterClient)
        {
            _logger = logger;
            _clusterClient = clusterClient;
        }

        public OperaResponseBody Ping(string s)
        {
            _logger.LogDebug("Exec ping method");
            return new OperaResponseBody(Flag.SUCCESS);
        }

        public OperaResponseBody RoomStatusUpdateBE(string body)
        {
            _logger.LogDebug("Exec RoomStatusUpdateBE method");
            _logger.LogDebug("Submit Message: {body}", body);

            try
            {
                int.TryParse(HotelId, out int hotelIdInt);
                var submitProducer = _clusterClient.GetGrain<ISubmitMessageProducerOracleCloud>(hotelIdInt);
                var response = submitProducer.ProduceRoomStatusUpdateBE(body);

                _logger.LogDebug("SOAP Result: {result}", response.Result);

                var result = response.Result.IsSuccessful ? Flag.SUCCESS : Flag.FAIL;

                return new OperaResponseBody(result);
            }
            catch (Exception ex)
            {
                return new OperaResponseBody(ex);
            }
        }

        public OperaResponseBody QueueRoomBE(string body)
        {
            _logger.LogDebug("Exec QueueRoomBE method");
            _logger.LogDebug("Submit Message: {body}", body);

            try
            {
                int.TryParse(HotelId, out int hotelIdInt);
                var submitProducer = _clusterClient.GetGrain<ISubmitMessageProducerOracleCloud>(hotelIdInt);
                var response = submitProducer.ProduceQueueRoomBE(body);

                _logger.LogDebug("SOAP Result: {result}", response.Result);

                var result = response.Result.IsSuccessful ? Flag.SUCCESS : Flag.FAIL;

                return new OperaResponseBody(result);
            }
            catch (Exception ex)
            {
                return new OperaResponseBody(ex);
            }
        }

        public OperaResponseBody GuestStatusNotificationExt(string body)
        {
            _logger.LogDebug("Exec GuestStatusNotificationExt method");
            _logger.LogDebug("Submit Message: {body}", body);

            try
            {
                int.TryParse(HotelId, out int hotelIdInt);
                var submitProducer = _clusterClient.GetGrain<ISubmitMessageProducerOracleCloud>(hotelIdInt);
                var response = submitProducer.ProduceGuestStatusNotificationExt(body);

                _logger.LogDebug("SOAP Result: {result}", response.Result);

                var result = response.Result.IsSuccessful ? Flag.SUCCESS : Flag.FAIL;

                return new OperaResponseBody(result);
            }
            catch (Exception ex)
            {
                return new OperaResponseBody(ex);
            }
        }

        public OperaResponseBody NewProfile(string body)
        {
            _logger.LogDebug("Exec NewProfile method");
            _logger.LogDebug("Submit Message: {body}", body);

            try
            {
                int.TryParse(HotelId, out int hotelIdInt);
                var submitProducer = _clusterClient.GetGrain<ISubmitMessageProducerOracleCloud>(hotelIdInt);
                var response = submitProducer.ProduceNewProfile(body);
                _logger.LogDebug("SOAP Result: {result}", response.Result);

                var result = response.Result.IsSuccessful ? Flag.SUCCESS : Flag.FAIL;

                return new OperaResponseBody(result);
            }
            catch (Exception ex)
            {
                return new OperaResponseBody(ex);
            }
        }

        public OperaResponseBody UpdateProfile(string body)
        {
            _logger.LogDebug("Exec UpdateProfile method");
            _logger.LogDebug("Submit Message: {body}", body);

            try
            {
                int.TryParse(HotelId, out int hotelIdInt);
                var submitProducer = _clusterClient.GetGrain<ISubmitMessageProducerOracleCloud>(hotelIdInt);
                var response = submitProducer.ProduceUpdateProfile(body);

                _logger.LogDebug("SOAP Result: {result}", response.Result);

                var result = response.Result.IsSuccessful ? Flag.SUCCESS : Flag.FAIL;

                return new OperaResponseBody(result);
            }
            catch (Exception ex)
            {
                return new OperaResponseBody(ex);
            }
        }
    }
}

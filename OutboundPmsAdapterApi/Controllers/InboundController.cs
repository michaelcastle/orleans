using Microsoft.Extensions.Logging;
using Optii.PMS.OperaCloud.Models;
using ServiceExtensions.Orleans;
using ServiceExtensions.PmsAdapter.SubmitMessage;
using ServiceExtensions.Soap.Core.Oasis;
using ServiceExtensions.Soap.Oasis;
using System;

namespace PmsAdapter.Api.Controllers.Opera
{
    public class InboundController : IOperaCloudService, IOperaCloudServiceProfile, IOperaCloudServiceGuestStatusNotification
    {
        private readonly ILogger<InboundController> _logger;
        private readonly ISubmitMessageHandlerOracleCloud _submitMessageHandler;
        private readonly IOasisSecurityService _securityObjectService;

        // TODO get this somehow?
        private const string HotelId = "1";

        public System.ServiceModel.Channels.MessageHeaders MessageHeaders { get; set; }

        public InboundController(IOasisSecurityService securityObjectService, ILogger<InboundController> logger, ISubmitMessageHandlerOracleCloud submitMessageHandler)
        {
            _logger = logger;
            _submitMessageHandler = submitMessageHandler;
            _securityObjectService = securityObjectService;
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
                var oasisSecurity = _securityObjectService.GetOasisSecurity(MessageHeaders);

                var response = _submitMessageHandler.SubmitRoomStatusUpdateBE(new SubmitMessage
                {
                    HotelId = HotelId,
                    Message = body,
                    Username = oasisSecurity.UsernameToken.Username,
                    Password = oasisSecurity.UsernameToken.Password
                });

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
                var oasisSecurity = _securityObjectService.GetOasisSecurity(MessageHeaders);
                var response = _submitMessageHandler.SubmitQueueRoomBE(new SubmitMessage
                {
                    HotelId = HotelId,
                    Message = body,
                    Username = oasisSecurity.UsernameToken.Username,
                    Password = oasisSecurity.UsernameToken.Password
                });

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
                var oasisSecurity = _securityObjectService.GetOasisSecurity(MessageHeaders);
                var response = _submitMessageHandler.SubmitGuestStatusNotificationExt(new SubmitMessage
                {
                    HotelId = HotelId,
                    Message = body,
                    Username = oasisSecurity.UsernameToken.Username,
                    Password = oasisSecurity.UsernameToken.Password
                });

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
                var oasisSecurity = _securityObjectService.GetOasisSecurity(MessageHeaders);
                var response = _submitMessageHandler.SubmitNewProfile(new SubmitMessage
                {
                    HotelId = HotelId,
                    Message = body,
                    Username = oasisSecurity.UsernameToken.Username,
                    Password = oasisSecurity.UsernameToken.Password
                });

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
                var oasisSecurity = _securityObjectService.GetOasisSecurity(MessageHeaders);
                var response = _submitMessageHandler.SubmitUpdateProfile(new SubmitMessage
                {
                    HotelId = HotelId,
                    Message = body,
                    Username = oasisSecurity.UsernameToken.Username,
                    Password = oasisSecurity.UsernameToken.Password
                });

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

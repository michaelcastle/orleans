using Microsoft.Extensions.Logging;
using Optii.PMS.OperaCloud.Models;
using ServiceExtensions.PmsAdapter.SubmitMessage;
using ServiceExtensions.Soap.Core.Oasis;
using ServiceExtensions.Soap.Oasis;
using System;

namespace PmsAdapter.Api.Controllers.Opera
{
    public class InboundController : IOperaCloudService, IOperaCloudServiceProfile, IOperaCloudServiceGuestStatusNotification
    {
        private readonly ILogger<InboundController> _logger;
        private readonly ISubmitMessageHandler _submitMessageHandler;
        private readonly IOasisSecurityService _securityObjectService;

        public System.ServiceModel.Channels.MessageHeaders MessageHeaders { get; set; }

        public InboundController(IOasisSecurityService securityObjectService, ILogger<InboundController> logger, ISubmitMessageHandler submitMessageHandler)
        {
            _logger = logger;
            _submitMessageHandler = submitMessageHandler;
            _securityObjectService = securityObjectService;
        }
        private OperaResponseBody Submit(string body, int hotelId = -1)
        {
            _logger.LogDebug("Submit Message: {body}", body);
            try
            {
                var oasisSecurity = _securityObjectService.GetOasisSecurity(MessageHeaders);
                var response = _submitMessageHandler.Submit(new SubmitMessage
                {
                    HotelId = hotelId.ToString(),
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

        public OperaResponseBody Ping(string s)
        {
            _logger.LogDebug("Exec ping method");
            return new OperaResponseBody(Flag.SUCCESS);
        }

        public OperaResponseBody RoomStatusUpdateBE(string body)
        {
            _logger.LogDebug("Exec RoomStatusUpdateBE method");
            return Submit(body);
        }

        public OperaResponseBody QueueRoomBE(string body)
        {
            _logger.LogDebug("Exec QueueRoomBE method");
            return Submit(body);
        }

        public OperaResponseBody GuestStatusNotificationExt(string body)
        {
            _logger.LogDebug("Exec GuestStatusNotificationExt method");
            return Submit(body);
        }

        public OperaResponseBody NewProfile(string body)
        {
            _logger.LogDebug("Exec NewProfile method");
            return Submit(body);
        }

        public OperaResponseBody UpdateProfile(string body)
        {
            _logger.LogDebug("Exec UpdateProfile method");
            return Submit(body);
        }
    }
}

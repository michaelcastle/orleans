using System;
using ServiceExtensions.PmsAdapter.SubmitMessage;
using ServiceExtensions.Soap.Core.Oasis;
using ServiceExtensions.Soap.Oasis;

namespace OutboundAdapter.Interfaces.Opera
{
    public static class OperaResponseSubmitExtensions
    {
        public static OperaResponseBody Submit(this ISubmitMessageHandler submitMessageHandler, IOasisSecurityService securityObjectService, System.ServiceModel.Channels.MessageHeaders headers, string body)
        {
            try
            {
                var oasisSecurity = securityObjectService.GetOasisSecurity(headers);
                var submitMessage = new SubmitMessage
                {
                    Message = body,
                    Username = oasisSecurity.UsernameToken.Username,
                    Password = oasisSecurity.UsernameToken.Password
                };
                var response = submitMessageHandler.Submit(submitMessage).Result;

                var result = response.IsSuccessful ? Flag.SUCCESS : Flag.FAIL;

                return new OperaResponseBody(result);
            }
            catch (Exception ex)
            {
                return new OperaResponseBody(ex);
            }
        }
    }
}

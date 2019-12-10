using Microsoft.Extensions.Logging;
using ServiceExtensions.PmsAdapter.SignIn;
using ServiceExtensions.PmsAdapter.SignIn.CachedLogin;
using ServiceExtensions.PmsAdapter.ClientChannel;
using System;
using System.ServiceModel;
using System.Threading.Tasks;
using ServiceExtensions.PmsAdapter.Connected_Services.PmsProcessor;

namespace ServiceExtensions.PmsAdapter.PmsProcessor
{
    public class PmsProcessorService : IPmsProcessorService
    {
        private const string LastAction = "External Login";

        private readonly ILogger<PmsProcessorService> _logger;
        private readonly ICachedExternalLogin _cachedLoginService;
        private readonly IClientChannelFactory<IPMSInterfaceContractChannel> _clientFactory;

        public PmsProcessorService(ILogger<PmsProcessorService> logger, ICachedExternalLogin cachedLoginService, IClientChannelFactory<IPMSInterfaceContractChannel> clientFactory)
        {
            _logger = logger;
            _cachedLoginService = cachedLoginService;
            _clientFactory = clientFactory;
        }

        public async Task<bool> SubmitMessage(string username, string password, string messageString, string hotelId)
        {
            var userSession = _cachedLoginService.ExternalLogin(username, password, LastAction, hotelId);
            if (userSession == null || !userSession.IsAuthorised || userSession.SessionId == Guid.Empty)
            {
                _logger.LogError("Failed login to PmsProcessor, check settings or credentials");
                return false;
            }

            var interfaceReturn = await ProcessSubmitMessage(userSession, password, messageString, hotelId);
            var result = interfaceReturn.ReturnType == InterfaceReturn.enReturnType.Success;
            if (!result)
            {
                _logger.LogError("Error: [{AbsolutePath}] SubmitMessage\n{messageString}\nReturnType: '{ReturnType}', InterfaceReturn: '{@interfaceReturn}'", _clientFactory.EndPoint.Address.Uri.AbsolutePath, messageString, interfaceReturn.ReturnType, interfaceReturn.FailureReason);
                throw new Exception(interfaceReturn.FailureReason);
            }

            return true;
        }

        private async Task<InterfaceReturn> ProcessSubmitMessage(SessionItem userSessionItem, string password, string messageString, string hotelId)
        {
            var client = _clientFactory.CreateChannel();
            
            try
            {
                client.Open();
                var interfaceReturn = await client.SubmitMessageAsync(userSessionItem.SessionId, messageString);
                if (interfaceReturn.ReturnType != InterfaceReturn.enReturnType.SessionEnded)
                    return interfaceReturn;

                var userSession = _cachedLoginService.ExternalLoginNoCache(userSessionItem.UserName, password, LastAction, hotelId);
                if (userSession == null || !userSession.IsAuthorised || userSession.SessionId == Guid.Empty)
                {
                    return new InterfaceReturn
                    {
                        ReturnType = InterfaceReturn.enReturnType.Failure,
                        FailureReason = "Could not login to PmsProcessor with no cache, check settings or credentials"
                    };
                }

                // don't re-attempt, a failure message means it's been delivered to the server but the server is unable to process it.
                // it's up to the server to handle that type of failure.
                return await client.SubmitMessageAsync(userSession.SessionId, messageString);
            }
            // If there is an exception we need to capture it and also close down the client to avoid memory leaks
            catch (NullReferenceException ex)
            {
                return new InterfaceReturn
                {
                    ReturnType = InterfaceReturn.enReturnType.Failure,
                    FailureReason = ex.Message
                };
            }
            catch (TimeoutException ex)
            {
                return new InterfaceReturn
                {
                    ReturnType = InterfaceReturn.enReturnType.Failure,
                    FailureReason = ex.Message
                };
            }
            catch (CommunicationException ex)
            {
                return new InterfaceReturn
                {
                    ReturnType = InterfaceReturn.enReturnType.Failure,
                    FailureReason = ex.Message
                };
            }
            catch (Exception ex)
            {
                return new InterfaceReturn
                {
                    ReturnType = InterfaceReturn.enReturnType.Failure,
                    FailureReason = ex.Message
                };

            }
            finally
            {
                _clientFactory.CloseChannel(client);
            }
        }
    }
}

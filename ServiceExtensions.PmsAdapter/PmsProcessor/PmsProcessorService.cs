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
        private readonly ILogger<PmsProcessorService> _logger;
        private readonly ICachedExternalLogin _cachedLoginService;

        public PmsProcessorService(ILogger<PmsProcessorService> logger, ICachedExternalLogin cachedLoginService)
        {
            _logger = logger;
            _cachedLoginService = cachedLoginService;
        }

        public async Task<bool> SubmitMessage(IClientChannelFactory<IPMSInterfaceContractChannel> clientFactory, string username, string password, string messageString)
        {
            var userSession = await _cachedLoginService.ExternalLogin(clientFactory, username, password);
            if (userSession == null || !userSession.IsAuthorised || userSession.SessionId == Guid.Empty)
            {
                _logger.LogError("Failed login to PmsProcessor, check settings or credentials");
                return false;
            }

            var interfaceReturn = await ProcessSubmitMessage(clientFactory, userSession, password, messageString);
            var result = interfaceReturn.ReturnType == InterfaceReturn.enReturnType.Success;
            if (!result)
            {
                _logger.LogError("Error: [{AbsolutePath}] SubmitMessage\n{messageString}\nReturnType: '{ReturnType}', InterfaceReturn: '{@interfaceReturn}'", clientFactory.EndPoint.Address.Uri.AbsolutePath, messageString, interfaceReturn.ReturnType, interfaceReturn.FailureReason);
                throw new Exception(interfaceReturn.FailureReason);
            }

            return true;
        }

        private async Task<InterfaceReturn> ProcessSubmitMessage(IClientChannelFactory<IPMSInterfaceContractChannel> clientFactory, SessionItem userSessionItem, string password, string messageString)
        {
            var client = clientFactory.CreateChannel();
            
            try
            {
                client.Open();
                var interfaceReturn = await client.SubmitMessageAsync(userSessionItem.SessionId, messageString);
                if (interfaceReturn.ReturnType != InterfaceReturn.enReturnType.SessionEnded)
                    return interfaceReturn;

                var userSession = await _cachedLoginService.ExternalLoginNoCache(clientFactory, userSessionItem.UserName, password);
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
                clientFactory.CloseChannel(client);
            }
        }
    }
}

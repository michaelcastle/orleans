using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ServiceExtensions.PmsAdapter.ClientChannel;
using ServiceExtensions.PmsAdapter.Connected_Services.PmsProcessor;
using ServiceExtensions.PmsAdapter.PmsProcessor;

namespace ServiceExtensions.PmsAdapter.SubmitMessage.Direct
{
    public class InboundPmsAdapter : ISubmitMessageHandler
    {
        private readonly ILogger<ISubmitMessageHandler> _logger;
        private readonly IPmsProcessorService _pmsProcessorService;
        private readonly IClientChannelFactory<IPMSInterfaceContractChannel> _clientFactory;

        public InboundPmsAdapter(ILogger<ISubmitMessageHandler> logger, IPmsProcessorService pmsProcessorService, IClientChannelFactory<IPMSInterfaceContractChannel> clientFactory)
        {
            _logger = logger;
            _pmsProcessorService = pmsProcessorService;
            _clientFactory = clientFactory;
        }

        public async Task<SubmitMessageResponse> Submit(SubmitMessage submit)
        {
            _logger.LogDebug("SubmitMessageDirect: {submitMessage}", submit.Message);

            try
            {
                var success = await _pmsProcessorService.SubmitMessage(_clientFactory, submit.Username, submit.Password, submit.Message);
                return new SubmitMessageResponse
                {
                    IsSuccessful = success
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fail publish to optii: {Message}", submit.Message);
                return new SubmitMessageResponse
                {
                    IsSuccessful = false,
                    FailReason = ex
                };
            }
        }
    }
}

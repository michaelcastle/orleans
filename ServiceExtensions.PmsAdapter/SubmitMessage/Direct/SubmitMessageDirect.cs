using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ServiceExtensions.PmsAdapter.PmsProcessor;

namespace ServiceExtensions.PmsAdapter.SubmitMessage.Direct
{
    public class InboundPmsAdapter : ISubmitMessageHandler
    {
        private readonly ILogger<ISubmitMessageHandler> _logger;
        private readonly IPmsProcessorService _pmsProcessorService;

        public InboundPmsAdapter(ILogger<ISubmitMessageHandler> logger, IPmsProcessorService pmsProcessorService)
        {
            _logger = logger;
            _pmsProcessorService = pmsProcessorService;
        }

        public async Task<SubmitMessageResponse> Submit(SubmitMessage submit)
        {
            _logger.LogDebug("SubmitMessageDirect: {submitMessage}", submit.Message);

            try
            {
                var success = await _pmsProcessorService.SubmitMessage(submit.Username, submit.Password, submit.Message, submit.HotelId);
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

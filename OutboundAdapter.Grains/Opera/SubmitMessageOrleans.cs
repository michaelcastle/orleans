using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Streams;
using OutboundAdapter.Interfaces;
using OutboundAdapter.Interfaces.Models;
using ServiceExtensions.PmsAdapter.SubmitMessage;

namespace OutboundAdapter.Grains.Opera
{
    public class SubmitMessageOrleans : ISubmitMessageHandler
    {
        private readonly ILogger<SubmitMessageOrleans> _logger;
        private readonly IClusterClient _clusterClient;
        private readonly IStreamProvider _streamProvider;

        public SubmitMessageOrleans(ILogger<SubmitMessageOrleans> logger, IClusterClient clusterClient, IStreamProvider streamProvider)
        {
            _logger = logger;
            _clusterClient = clusterClient;
            _streamProvider = streamProvider;
        }

        public Task<SubmitMessageResponse> Submit(SubmitMessage submitMessage)
        {
            return Task.FromResult(SubmitRoomStatusUpdateBE(submitMessage).Result);
        }

        public async Task<SubmitMessageResponse> SubmitRoomStatusUpdateBE(SubmitMessage submit)
        {
            _logger.LogDebug("SubmitMessageDirect: {submitMessage}", submit.Message);

            try
            {
                int.TryParse(submit.HotelId, out int hotelIdInt);
                var hotel = _clusterClient.GetGrain<IHotelPmsGrain>(hotelIdInt);
                if (!await hotel.IsConnected())
                {
                    return new SubmitMessageResponse
                    {
                        IsSuccessful = false,
                        FailReason = new Exception("Hotel is not connected to the PMS. Please Connect first.")
                    };
                }

                var streamNamespace = await hotel.StreamNamespace<RoomStatusUpdate>();
                var stream = _streamProvider.GetStream<string>(hotel.GetPrimaryKey(), streamNamespace);

                var streamed = stream.OnNextAsync(submit.Message);
                await streamed;

                if (streamed.IsFaulted)
                {
                    return new SubmitMessageResponse
                    {
                        IsSuccessful = false,
                        FailReason = streamed.Exception
                    };
                }

                return new SubmitMessageResponse
                {
                    IsSuccessful = streamed.IsCompletedSuccessfully
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

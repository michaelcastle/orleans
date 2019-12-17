using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Streams;
using OutboundAdapter.Interfaces;
using OutboundAdapter.Interfaces.Models;
using OutboundAdapter.Interfaces.Opera;
using OutboundAdapter.Interfaces.Opera.Models;
using ServiceExtensions.PmsAdapter.SubmitMessage;

namespace ServiceExtensions.Orleans
{
    public class SubmitMessageOrleans : ISubmitMessageHandlerOracleCloud
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

        public async Task<SubmitMessageResponse> SubmitRoomStatusUpdateBE(SubmitMessage submit)
        {
            return await SubmitToSubscribers<RoomStatusUpdateBERequestDto>(submit);
        }

        public async Task<SubmitMessageResponse> SubmitGuestStatusNotificationExt(SubmitMessage submit)
        {
            return await SubmitToSubscribers<GuestStatusNotificationExtRequestDto>(submit);
        }

        public async Task<SubmitMessageResponse> SubmitQueueRoomBE(SubmitMessage submit)
        {
            return await SubmitToSubscribers<QueueRoomBERequestDto>(submit);
        }

        public async Task<SubmitMessageResponse> SubmitNewProfile(SubmitMessage submit)
        {
            return await SubmitToSubscribers<NewProfileRequestDto>(submit);
        }

        public async Task<SubmitMessageResponse> SubmitUpdateProfile(SubmitMessage submit)
        {
            return await SubmitToSubscribers<UpdateProfileRequestDto>(submit);
        }

        private async Task<SubmitMessageResponse> SubmitToSubscribers<T>(SubmitMessage submit)
        {
            _logger.LogDebug("SubmitToSubscribers: {submitMessage}", submit.Message);

            try
            {
                int.TryParse(submit.HotelId, out int hotelIdInt);
                var hotel = _clusterClient.GetGrain<IHotelPmsGrain>(hotelIdInt);

                var streamNamespace = await hotel.StreamNamespaceInbound<T, Constants.Outbound.OperaCloud>();
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

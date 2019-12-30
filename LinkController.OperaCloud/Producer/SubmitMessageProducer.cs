using LinkController.OperaCloud.Interfaces;
using LinkController.OperaCloud.Interfaces.Models;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Streams;
using OutboundAdapter.Interfaces;
using OutboundAdapter.Interfaces.StreamHelpers;
using ServiceExtensions.PmsAdapter.SubmitMessage;
using System;
using System.Threading.Tasks;

namespace LinkController.OperaCloud.Grains.Producer
{
    public class SubmitMessageProducer : Grain, ISubmitMessageProducerOracleCloud
    {
        private const string StreamProviderName = "SMSProvider";
        private readonly ILogger<SubmitMessageProducer> _logger;
        private readonly IClusterClient _clusterClient;
        private readonly IStreamProvider _streamProvider;
        private readonly IStreamNamespaces _streamNamspaces;

        public SubmitMessageProducer(ILogger<SubmitMessageProducer> logger, IClusterClient clusterClient, IStreamNamespaces streamNamspaces)
        {
            _logger = logger;
            _clusterClient = clusterClient;
            _streamProvider = clusterClient.GetStreamProvider(StreamProviderName);
            _streamNamspaces = streamNamspaces;
        }

        private int HotelId => (int)this.GetPrimaryKeyLong();

        public async Task<SubmitMessageResponse> ProduceRoomStatusUpdateBE(string body)
        {
            return await Produce<RoomStatusUpdateBERequestDto>(body);
        }

        public async Task<SubmitMessageResponse> ProduceGuestStatusNotificationExt(string body)
        {
            return await Produce<GuestStatusNotificationExtRequestDto>(body);
        }

        public async Task<SubmitMessageResponse> ProduceQueueRoomBE(string body)
        {
            return await Produce<QueueRoomBERequestDto>(body);
        }

        public async Task<SubmitMessageResponse> ProduceNewProfile(string body)
        {
            return await Produce<NewProfileRequestDto>(body);
        }

        public async Task<SubmitMessageResponse> ProduceUpdateProfile(string body)
        {
            return await Produce<UpdateProfileRequestDto>(body);
        }

        private async Task<SubmitMessageResponse> Produce<T>(string body)
        {
            _logger.LogDebug("SubmitToSubscribers: {submitMessage}", body);

            try
            {
                var hotel = _clusterClient.GetGrain<IHotelPmsGrain>(HotelId);
                var streamNamespace = _streamNamspaces.InboundNamespace<T, Constants.Outbound.OperaCloud>();
                var stream = _streamProvider.GetStream<string>(hotel.GetPrimaryKey(), streamNamespace);

                var streamed = stream.OnNextAsync(body);
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
                _logger.LogError(ex, "Fail publish to optii: {Message}", body);
                return new SubmitMessageResponse
                {
                    IsSuccessful = false,
                    FailReason = ex
                };
            }
        }
    }
}

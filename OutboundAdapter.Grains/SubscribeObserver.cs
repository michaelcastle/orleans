using Orleans;
using Orleans.Runtime;
using Orleans.Streams;
using OutboundAdapter.Interfaces;
using OutboundAdapter.Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OutboundAdapter.Grains
{
    public class SubscribeObserver<TMessage> : Grain, ISubscribeObserver
    {
        private readonly List<IAsyncStream<TMessage>> _streams = new List<IAsyncStream<TMessage>>();
        private readonly List<StreamSubscriptionHandle<TMessage>> _handles = new List<StreamSubscriptionHandle<TMessage>>();
        protected readonly IPersistentState<SubscribeEndpoint> _configuration;

        public SubscribeObserver(IPersistentState<SubscribeEndpoint> configuration)
        {
            _configuration = configuration;
        }

        public Guid GetPrimaryKey()
        {
            var primaryKey = this.GetPrimaryKey(out string keyExtension);
            return primaryKey;
        }

        public long GetPrimaryKeyLong()
        {
            var primaryKey = this.GetPrimaryKeyLong(out string keyExtension);
            return primaryKey;
        }

        protected string[] StreamNamespaces { get; set; } = { "" };
        protected string StreamProvider { get; set; } = "SMSProvider";

        public override async Task OnActivateAsync()
        {
            var streamProvider = GetStreamProvider(StreamProvider);

            foreach (var streamNamespace in StreamNamespaces)
            {
                await AddSubscription(streamNamespace, streamProvider);
            }

            await base.OnActivateAsync();
        }

        public async Task BecomeConsumer(Guid streamId, string streamNamespace, string providerToUse)
        {
            StreamProvider = providerToUse;

            var stream = FindSubscriptionStream(streamNamespace, providerToUse);
            if (stream != null)
            {
                await RemoveSubscription(stream);
            }

            var streamProvider = GetStreamProvider(providerToUse);
            await AddSubscription(streamNamespace, streamProvider);
        }

        private IAsyncStream<TMessage> FindSubscriptionStream(string streamNamespace, string providerToUse)
        {
            return _streams.FirstOrDefault(x => x.ProviderName == providerToUse && x.Namespace == streamNamespace);
        }

        private async Task RemoveSubscription(IAsyncStream<TMessage> stream)
        {
            var handle = _handles.FirstOrDefault(x => x.StreamIdentity.Guid == stream.Guid);

            await handle.UnsubscribeAsync();
            _handles.Remove(handle);
            handle = null;

            _streams.Remove(stream);
            stream = null;
        }

        private async Task AddSubscription(string streamNamespace, IStreamProvider streamProvider)
        {
            var stream = streamProvider.GetStream<TMessage>(this.GetPrimaryKey(), streamNamespace);
            _streams.Add(stream);
            _handles.Add(await stream.SubscribeAsync(OnNextAsync, OnErrorAsync, OnCompletedAsync));
        }

        public async Task SetConfiguration(ISubscribeEndpoint configuration)
        {
            _configuration.State = (SubscribeEndpoint)configuration;
            await _configuration.WriteStateAsync();
        }

        public async Task StopConsuming()
        {
            if (!_handles.Any())
            {
                return;
            }

            var tasks = new List<Task>();
            foreach (var handle in _handles)
            {
                tasks.Add(handle.UnsubscribeAsync());
            }
            await Task.WhenAll(tasks);

            _handles.Clear();
            _streams.Clear();
        }

        public virtual Task OnNextAsync(TMessage item, StreamSequenceToken token = null)
        {
            return Task.CompletedTask;
        }

        public virtual Task OnCompletedAsync()
        {
            return Task.CompletedTask;
        }

        public virtual Task OnErrorAsync(Exception ex)
        {
            return Task.CompletedTask;
        }
    }
}

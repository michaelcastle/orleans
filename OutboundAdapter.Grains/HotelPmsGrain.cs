using Orleans;
using OutboundAdapter.Interfaces;
using OutboundAdapter.Interfaces.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OutboundAdapter.Grains
{
    public class HotelPmsGrain : Grain, IHotelPmsGrain
    {
         public async Task UnsubscribeHtng<T>(ISubscribeEndpoint configuration) where T : IGrain, ISubscribeWithNamespaceObserver
        {
            var tasks = new List<Task>();
            var consumer = GrainFactory.GetGrain<T>((int)this.GetPrimaryKeyLong(), configuration.CompoundKeyEndpoint());
            tasks.Add(consumer.StopConsuming());
            await Task.WhenAll(tasks);
        }

        public async Task Subscribe<T>(string provider, ISubscribeEndpoint configuration) where T : IGrain, ISubscribeWithNamespaceObserver
        {
            var roomStatusUpdateBeObserver = GrainFactory.GetGrain<T>((int)this.GetPrimaryKeyLong(), configuration.CompoundKeyEndpoint(), null);
            await roomStatusUpdateBeObserver.SetConfiguration(configuration);
            await SubscribeResponses(provider, new List<string> { await roomStatusUpdateBeObserver.Namespace() }, roomStatusUpdateBeObserver);
        }

        public async Task SubscribeResponses(string provider, IList<string> streamNamespaces, ISubscribeObserver observer)
        {
            var tasks = new List<Task>();

            foreach (var streamNamespace in streamNamespaces)
            {
                tasks.Add(observer.BecomeConsumer(this.GetPrimaryKey(), streamNamespace, provider));
            }

            await Task.WhenAll(tasks);
        }
    }
}

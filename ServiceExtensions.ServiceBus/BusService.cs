using MassTransit;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceExtensions.ServiceBus
{
    public class RabbitMqBusService : IHostedService
    {
        private readonly IBusControl _busControl;

        public RabbitMqBusService(IBusControl busControl, IPublishObserver publishObserver)
        {
            _busControl = busControl;
            _busControl.ConnectPublishObserver(publishObserver);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return _busControl.StartAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return _busControl.StopAsync(cancellationToken);
        }
    }

    public class InMemoryBusService : IHostedService
    {
        private readonly IBusControl _busControl;

        public InMemoryBusService(IBusControl busControl)
        {
            _busControl = busControl;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return _busControl.StartAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return _busControl.StopAsync(cancellationToken);
        }
    }
}
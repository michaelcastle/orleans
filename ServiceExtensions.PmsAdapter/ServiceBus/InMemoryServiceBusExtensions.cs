using GreenPipes;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ServiceExtensions.PmsAdapter.SubmitMessage.ToQueue;
using System;
using ServiceExtensions.ServiceBus;

namespace ServiceExtensions.PmsAdapter.ServiceBus
{
    public static class InMemoryServiceBusExtensions
    {
        public static IServiceCollection AddMassTransitInMemoryServiceBus(this IServiceCollection serviceCollection, string inMemoryQueueName)
        {
            serviceCollection.AddMassTransit(configure =>
            {
                configure.AddConsumer<SubmitMessageConsumer>(cc => cc.UseConcurrentMessageLimit(2));
                configure.AddConsumer<SubmitMessageFaultConsumer>();

                configure.AddBusUsingInMemory(inMemoryQueueName);
            });

            serviceCollection.AddSingleton<Microsoft.Extensions.Hosting.IHostedService, InMemoryBusService>();

            return serviceCollection;
        }

        private static MassTransit.ExtensionsDependencyInjectionIntegration.IServiceCollectionConfigurator AddBusUsingInMemory(this MassTransit.ExtensionsDependencyInjectionIntegration.IServiceCollectionConfigurator bus, string inMemoryQueueName)
        {
            bus.AddBus(provider => Bus.Factory.CreateUsingInMemory(configure =>
            {
                configure.SetLoggerFactory(provider.GetService<ILoggerFactory>());
                configure.ReceiveEndpoint(inMemoryQueueName, endpoint =>
                {
                    //endpoint.UseScheduledRedelivery(r => r.Intervals(TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(7), TimeSpan.FromMinutes(15)));
                    //endpoint.UseMessageRetry(r => r.Immediate(5));
                    endpoint.UseCircuitBreaker(circuitBreaker =>
                    {
                        circuitBreaker.TrackingPeriod = TimeSpan.FromMinutes(1);
                        circuitBreaker.TripThreshold = 15;
                        circuitBreaker.ActiveThreshold = 10;
                        circuitBreaker.ResetInterval = TimeSpan.FromMinutes(5);
                    });
                    endpoint.ConfigureConsumer<SubmitMessageConsumer>(provider);
                    endpoint.ConfigureConsumer<SubmitMessageFaultConsumer>(provider);
                });
            }));

            return bus;
        }
    }
}

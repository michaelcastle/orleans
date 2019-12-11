using GreenPipes;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ServiceExtensions.PmsAdapter.SubmitMessage;
using ServiceExtensions.PmsAdapter.SubmitMessage.ToQueue;
using ServiceExtensions.ServiceBus;
using System;

namespace ServiceExtensions.PmsAdapter.ServiceBus
{
    public static class RabbitMqServiceBusExtensions
    {
        public static IServiceCollection AddMassTransitRabbitMqServiceBus(this IServiceCollection serviceCollection)
        {
            var submitMessageSettings = serviceCollection.BuildServiceProvider()
                .GetService<IOptions<SubmitMessageSettings>>()
                .Value ?? new SubmitMessageSettings();

            serviceCollection.AddMassTransit(configure =>
            {
                configure.AddConsumer<SubmitMessageConsumer>(cc => cc.UseConcurrentMessageLimit(2));
                configure.AddConsumer<SubmitMessageFaultConsumer>();

                configure.AddBusUsingRabbitMq(submitMessageSettings);
            });

            serviceCollection.AddSingleton<Microsoft.Extensions.Hosting.IHostedService, RabbitMqBusService>();

            return serviceCollection;
        }

        private static MassTransit.ExtensionsDependencyInjectionIntegration.IServiceCollectionConfigurator AddBusUsingRabbitMq(this MassTransit.ExtensionsDependencyInjectionIntegration.IServiceCollectionConfigurator bus, SubmitMessageSettings submitMessageSettings)
        {
            bus.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(configure =>
            {
                var host = configure.Host(new Uri(submitMessageSettings.QueueSettings.RabbitMqSettings.Host ?? "rabbitmq://localhost"), hostConfigurator => {
                    hostConfigurator.Username(submitMessageSettings.QueueSettings.RabbitMqSettings.Username);
                    hostConfigurator.Password(submitMessageSettings.QueueSettings.RabbitMqSettings.Password);
                });

                configure.SetLoggerFactory(provider.GetService<ILoggerFactory>());
                configure.ReceiveEndpoint(submitMessageSettings.QueueSettings.QueueName ?? "opera_cloud", endpoint =>
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

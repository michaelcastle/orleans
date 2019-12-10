using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using ServiceExtensions.PmsAdapter.ClientChannel;
using ServiceExtensions.PmsAdapter.ServiceBus;
using System;
using System.ServiceModel;
using ServiceExtensions.PmsAdapter.SignIn;
using ServiceExtensions.PmsAdapter.SignIn.Authentication;
using Microsoft.Extensions.Configuration;
using ServiceExtensions.PmsAdapter.Connected_Services.PmsProcessor;
using ServiceExtensions.PmsAdapter.SubmitMessage;
using ServiceExtensions.PmsAdapter.SignIn.CachedLogin;
using ServiceExtensions.ServiceBus;

namespace ServiceExtensions.PmsAdapter.PmsProcessor
{
    public static class PmsProcessorExtensions
    {
        /// <summary>
        /// These services are the default services needed for connecting to the optii v2 pms processor api. Health checks can also be
        /// added to the optii v2 pms processor api with the AddPmsProcessorHealthChecks() extension
        /// <para>
        /// An <see cref="IServiceCollection"/> extension for enabling a connection to the optii v2 pms processor api.
        /// </para>
        /// <para>
        /// The <see cref="IConfiguration"/> required for connecting to the services can be found in the default app settings in the 
        /// PmsProcessorSettings section.
        /// </para>
        /// </summary>
        /// <example> 
        /// This sample shows the default way <see cref="AddPmsProcessor"/> method is used.
        /// <code>
        /// public void ConfigureServices(IServiceCollection services)
        /// {
        ///     services
        ///        .AddPmsProcessor();
        ///     
        ///     if (services.IsHealthChecksEnabled())  
        ///     {
        ///         services
        ///             .AddHealthCheckServices()
        ///             .AddPmsProcessorHealthChecks();
        ///     }
        ///     
        ///     services
        ///         .AddSerilogAndRequestResponseLogging();
        ///         
        /// </code>
        /// </example>
        public static IServiceCollection AddPmsProcessor(this IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddConfigurationPmsAdapterSettings();

            serviceCollection
                .AddPmsProcessorServices<PmsProcessorService, PmsProcessorSignInService>();

            serviceCollection.AddMemoryCache();
            serviceCollection.TryAddSingleton<ILoginCacheService, OptiiLoginCache>();
            serviceCollection.TryAddSingleton<ICachedExternalLogin, CachedExternalLogin>();

            return serviceCollection
                .AddSubmitMessageHandler();
        }

        public static IServiceCollection AddConfigurationPmsAdapterSettings(this IServiceCollection serviceCollection)
        {
            var configuration = serviceCollection.BuildServiceProvider()
                .GetService<IConfiguration>();

            return serviceCollection
                .Configure<RabbitMqSettings>(configuration.GetSection("PmsProcessorSettings:SubmitMessage:QueueSettings:RabbitMqSettings"))
                .Configure<SubmitMessageSettings>(configuration.GetSection("PmsProcessorSettings:SubmitMessage"))
                .Configure<EncryptionSettings>(configuration.GetSection("PmsProcessorSettings:Encryption"))
                .Configure<PmsProcessorSettings>(configuration.GetSection("PmsProcessorSettings"));
        }

        private static IServiceCollection AddWcfClientFactory(this IServiceCollection serviceCollection)
        {
            var pmsProcessorSettings = serviceCollection.BuildServiceProvider()
                .GetService<IOptions<PmsProcessorSettings>>()
                .Value ?? new PmsProcessorSettings();

            var endpoint = new EndpointAddress(new Uri(pmsProcessorSettings.Url));

            var clientFactory = endpoint.Uri.Scheme == "http" ? GetHttpClientFactory(endpoint) : GetHttpsClientFactory(endpoint);

            serviceCollection.TryAddSingleton<IClientChannelFactory<IPMSInterfaceContractChannel>>(clientFactory);

            return serviceCollection;
        }

        private static ClientChannelFactory<IPMSInterfaceContractChannel> GetHttpClientFactory(EndpointAddress endpoint)
        {
            var binding = new BasicHttpBinding();

            return new ClientChannelFactory<IPMSInterfaceContractChannel>(binding, endpoint);
        }

        private static ClientChannelFactory<IPMSInterfaceContractChannel> GetHttpsClientFactory(EndpointAddress endpoint)
        {
            var binding = new BasicHttpsBinding(BasicHttpsSecurityMode.Transport);
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;

            var clientFactory = new ClientChannelFactory<IPMSInterfaceContractChannel>(binding, endpoint);

            return clientFactory;
        }

        public static IServiceCollection AddOptiiAuthentication<TU>(this IServiceCollection serviceCollection)
            where TU : class, ISessionItemAuthenticationService
        {
            serviceCollection.TryAddSingleton<ISessionItemAuthenticationService, TU>();
            serviceCollection.TryAddSingleton<ISecurityAuthenticator, OptiiAuthenticator>();

            return serviceCollection;
        }

        public static IServiceCollection AddPmsProcessorServices<TP, TU>(this IServiceCollection serviceCollection)
            where TP : class, IPmsProcessorService
            where TU : class, ISessionItemAuthenticationService
        {
            serviceCollection.AddOptiiAuthentication<TU>();

            serviceCollection.TryAddSingleton<IPmsProcessorService, TP>();
            serviceCollection.AddWcfClientFactory();
            serviceCollection.AddSingleton<IPublishObserver, PublishObserver>();

            if (!serviceCollection.IsUsingBus())
            {
                return serviceCollection;
            }

            var submitMessageSettings = serviceCollection.BuildServiceProvider()
                .GetService<IOptions<SubmitMessageSettings>>()
                .Value ?? new SubmitMessageSettings();

            switch (submitMessageSettings.QueueSettings.QueueType)
            {
                case QueueType.RabbitMq:
                    return serviceCollection.AddMassTransitRabbitMqServiceBus();

                default:
                    return serviceCollection.AddMassTransitInMemoryServiceBus(submitMessageSettings.QueueSettings.QueueName);
            }
        }

        private static bool IsUsingRabbitMq(this IServiceCollection serviceCollection)
        {
            var submitMessageSettings = serviceCollection.BuildServiceProvider()
                .GetService<IOptions<SubmitMessageSettings>>()
                .Value ?? new SubmitMessageSettings();

            return submitMessageSettings.QueueSettings.QueueType == QueueType.RabbitMq;
        }

        private static bool IsUsingBus(this IServiceCollection serviceCollection)
        {
            var submitMessageSettings = serviceCollection.BuildServiceProvider()
                .GetService<IOptions<SubmitMessageSettings>>()
                .Value ?? new SubmitMessageSettings();

            return submitMessageSettings.SubmitType == SubmitType.ToQueue;
        }

        public static IHealthChecksBuilder AddPmsProcessorHealthChecks(this IHealthChecksBuilder healthChecksBuilder)
        {
            healthChecksBuilder
                .AddCheck<HealthCheck>("pms processor endpoint", failureStatus: HealthStatus.Degraded, tags: new[] { "ready" });

            if (!healthChecksBuilder.Services.IsUsingRabbitMq() || !healthChecksBuilder.Services.IsUsingBus())
            {
                return healthChecksBuilder;
            }

            var rabbitMqSettings = healthChecksBuilder.Services.BuildServiceProvider()
               .GetService<IOptions<RabbitMqSettings>>()
               .Value ?? new RabbitMqSettings();

            var rabbitMqHost = new Uri(rabbitMqSettings.Host);

            return healthChecksBuilder
                .AddRabbitMQ($"amqp://{rabbitMqSettings.Username}:{rabbitMqSettings.Password}@{rabbitMqHost.Host}{rabbitMqHost.AbsolutePath}", name: "rabbit mq", tags: new[] { "ready" });
        }
    }
}

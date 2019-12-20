using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using OutboundAdapter.Interfaces.StreamHelpers;
using Polly;
using System;

namespace LinkController.OperaCloud.Interfaces.OrleansClient
{
    public static class OrleansExtensions
    {
        public static IServiceCollection AddOrleansClient(this IServiceCollection services)
        {
            services.TryAddTransient<IStreamNamespaces, StreamNamespaces>();
            services.AddSingleton(provider =>
            {
                return Policy<IClusterClient>
                    .Handle<Exception>()
                    .WaitAndRetry(new[]
                    {
                        TimeSpan.FromSeconds(1),
                        TimeSpan.FromSeconds(2),
                        TimeSpan.FromSeconds(3)
                    })
                    .Execute(() => ConnectClient());
            });
            return services;
        }

        public static IApplicationBuilder UseOrleansClient(this IApplicationBuilder app)
        {
            app.ApplicationServices.GetService<IGrainFactory>();
            return app;
        }

        private static IClusterClient ConnectClient()
        {
            IClusterClient client;
            client = new ClientBuilder()
                .UseLocalhostClustering()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "dev";
                    options.ServiceId = "OperaPmsAdapter";
                })
                .AddSimpleMessageStreamProvider("SMSProvider",
                            options =>
                            {
                                options.OptimizeForImmutableData = false;
                                options.FireAndForgetDelivery = false;
                                //options.PubSubType = Orleans.Streams.StreamPubSubType.ImplicitOnly;
                            })
                .ConfigureLogging(logging => logging.AddConsole())
                .Build();

            //await client.Connect(RetryFilter);
            client.Connect().Wait();

            Console.WriteLine("Client successfully connected to silo host \n");

            return client;
        }

    }
}

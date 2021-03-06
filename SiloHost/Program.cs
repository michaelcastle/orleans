﻿using LinkController.OperaCloud.Grains;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using OutboundAdapter.Grains;
using SiloHost.OperaCloud;
using System;
using System.Threading.Tasks;

namespace SiloHost
{
    public class Program
    {
        public static int Main(string[] _)
        {
            return RunMainAsync().Result;
        }

        private static async Task<int> RunMainAsync()
        {
            try
            {
                var host = await StartSilo();
                Console.WriteLine("\n\n Press Enter to terminate...\n\n");
                Console.ReadLine();

                await host.StopAsync();

                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return 1;
            }
        }

        private static async Task<ISiloHost> StartSilo()
        {
            // define the cluster configuration
            var builder = new SiloHostBuilder()
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
                .ConfigureApplicationParts(parts =>
                {
                    parts.AddApplicationPart(typeof(HotelPmsGrain).Assembly).WithReferences();
                    //parts.AddApplicationPart(typeof(IAuthenticateOracleCloudGrains).Assembly).WithReferences();
                    parts.AddApplicationPart(typeof(AuthenticateOracleCloudGrains).Assembly).WithReferences();
                })
                //.UseDashboard(options => { options.HideTrace = true; })
                .AddMemoryGrainStorage(name: "PubSubStore")
                .AddMemoryGrainStorage(name: "oracleCloudStore")
                .AddAdoNetGrainStorage(name: "subscribeEndpointConfigurationStore", options =>
                {
                    options.Invariant = "System.Data.SqlClient";
                    options.ConnectionString = "Data Source=DESKTOP-J31L3H2;Initial Catalog=LinkController;Integrated Security=False;User ID=app;Password=app;";
                    options.UseJsonFormat = true;
                })
                .ConfigureServices(services =>
                {
                    services.AddOperaCloud();

                    ////https://www.stevejgordon.co.uk/introduction-to-httpclientfactory-aspnetcore
                    //var timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(10));
                    //services.AddHttpClient("Opera", client =>
                    //{
                    //    //client.DefaultRequestHeaders.Add("Content-Type", "text/xml");
                    //    //client.DefaultRequestHeaders.Add("SOAPAction", "http://webservices.micros.com/htng/2008B/SingleGuestItinerary#UpdateRoomStatus");
                    //})
                    //.AddPolicyHandler(timeoutPolicy)
                    //.AddTransientHttpErrorPolicy(p => p.RetryAsync(3));

                    //services.AddSingleton<IClock>(SystemClock.Instance);
                    //services.AddTransient<IOperaEnvelopeSerializer, OperaEnvelopeSerializer>();
                })
                .ConfigureLogging(logging => logging.AddConsole());

            var host = builder.Build();
            await host.StartAsync();
            return host;
        }
    }
}

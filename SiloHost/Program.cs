using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using OutboundAdapter.Grains;
using OutboundAdapter.Interfaces;
using OutboundAdapter.Interfaces.PmsClients;
using SiloHost.Clients;

namespace SiloHost
{
    public class Program
    {
        public static int Main(string[] args)
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
                .ConfigureApplicationParts(parts => {
                    parts.AddApplicationPart(typeof(OutboundAdapterGrain).Assembly).WithReferences();
                    //parts.AddApplicationPart(typeof(HotelPmsGrain).Assembly).WithReferences();
                    //parts.AddApplicationPart(typeof(OutboundMappingOperaGrains).Assembly).WithReferences();
                })
                //.UseDashboard(options => { options.HideTrace = true; })
                .AddMemoryGrainStorage(name: "hotelConfigurationStore")
                .ConfigureServices(services => { services.AddHttpClient(); })
                .ConfigureServices(services => { services.AddSingleton<IOperaHTNG2008BServiceClient, OperaHTNG2008BServiceClient>(); })
                .ConfigureServices(services => { services.AddSingleton<IOperaHTNG_EXT2008BWebServicesClient, OperaHTNG_EXT2008BWebServicesClient>(); })
                .ConfigureLogging(logging => logging.AddConsole());

            var host = builder.Build();
            await host.StartAsync();
            return host;
        }
    }
}

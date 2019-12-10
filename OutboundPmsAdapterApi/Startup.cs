using System;
using System.ServiceModel;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Optii.PMS.OperaCloud.Models;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using PmsAdapter.Api.Controllers.Opera;
using Polly;
using ServiceExtensions.Formatters.BodyRequestFormatters;
using ServiceExtensions.Soap.Core;

namespace PmsAdapter.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
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
            services.AddControllers();

            services.AddInboundPmsAdapter();

            services.AddMvc(o => o.InputFormatters.Insert(0, new RawRequestBodyFormatter()))
                   .AddXmlSerializerFormatters()
                   .AddXmlDataContractSerializerFormatters();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Put title here", Description = "DotNet Core Api 3 - with swagger" });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Outbound Api V1");
            });

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.ApplicationServices.GetService<IGrainFactory>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // Need to declare it using the implementation and not the interface to have it add the headers to the service call
            //app.UseSoapEndpoint<OperaCloudService>("/OperaCloudService.svc", new BasicHttpBinding(), SoapSerializer.DataContractSerializer);  // This deserializes Request Body to an object
            app
                .UseSoapEndpoint<InboundController>("/OperaCloudService.svc", new BasicHttpBinding(), SoapSerializer.StringBodyDataContractSerializer)  // This just extracts the Request Body as a string
                .UseSoapEndpoint<InboundController>("/OperaCloudService.asmx", new BasicHttpBinding(), SoapSerializer.XmlSerializer);
        }

        private IClusterClient ConnectClient()
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
                                options.PubSubType = Orleans.Streams.StreamPubSubType.ImplicitOnly;
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

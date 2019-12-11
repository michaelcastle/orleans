using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PmsAdapter.Api.Controllers.Opera;
using ServiceExtensions.Soap.Core;
using System.ServiceModel;

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
            services.AddOperaCloudLinkController();

            services.AddMvc(c => {
                c.EnableEndpointRouting = false;
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

            // Need to declare it using the implementation and not the interface to have it add the headers to the service call
            //app.UseSoapEndpoint<OperaCloudService>("/OperaCloudService.svc", new BasicHttpBinding(), SoapSerializer.DataContractSerializer);  // This deserializes Request Body to an object
            app
                .UseSoapEndpoint<InboundController>("/OperaCloudService.svc", new BasicHttpBinding(), SoapSerializer.StringBodyDataContractSerializer)  // This just extracts the Request Body as a string
                .UseSoapEndpoint<InboundController>("/OperaCloudService.asmx", new BasicHttpBinding(), SoapSerializer.XmlSerializer);
            app.UseMvc();

            app.UseOperaCloudLinkController();
        }
    }
}

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Orleans;
using PmsAdapter.Api.Controllers.Opera;
using ServiceExtensions.Orleans;
using ServiceExtensions.Soap.Core;
using ServiceExtensions.Soap.Oasis;
using System.ServiceModel;

namespace PmsAdapter.Api
{
    public static class LinkControllerExtensions
    {
        public static IServiceCollection AddOperaCloudLinkController(this IServiceCollection services)
        {
            // Orleans dependencies
            services.AddOrleansClient();
            services.TryAddSingleton(provider =>
            {
                var service = provider.GetRequiredService<IClusterClient>();
                return service.GetStreamProvider("SMSProvider");
            });
            services.TryAddSingleton<ISubmitMessageHandlerOracleCloud, SubmitMessageOrleans>();

            // Wcf/Soap dependencies
            services.AddXmlWriterNetCore3Fix();
            services.AddOptiiOasisSecurityFilter<OrleansSignInService>();

            // Service api controller
            services.TryAddSingleton<InboundController>();

            return services;
        }

        public static IServiceCollection AddXmlWriterNetCore3Fix(this IServiceCollection services)
        {
            // If using Kestrel:
            services.Configure<KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            // If using IIS:
            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });
            return services;
        }

        public static IApplicationBuilder UseOperaCloudLinkController(this IApplicationBuilder app)
        {
            app.UseOrleansClient();
            

            return app;
        }
    }
}

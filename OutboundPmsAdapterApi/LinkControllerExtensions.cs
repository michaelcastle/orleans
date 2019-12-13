using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Orleans;
using OutboundAdapter.Grains.Opera;
using PmsAdapter.Api.Controllers.Opera;
using ServiceExtensions.Orleans;
using ServiceExtensions.PmsAdapter.PmsProcessor;
using ServiceExtensions.PmsAdapter.SignIn.CachedLogin;
using ServiceExtensions.Soap.Core;
using ServiceExtensions.Soap.Core.Oasis;
using ServiceExtensions.Soap.Core.Response;
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
            services.AddOptiiOasisSecurityFilter();

            // Service api controller
            services.TryAddSingleton<InboundController>();

            return services;
        }

        public static IServiceCollection AddOptiiOasisSecurityFilter(this IServiceCollection serviceCollection)
        {
            var configuration = serviceCollection.BuildServiceProvider()
                .GetService<IConfiguration>();

            serviceCollection.Configure<OasisSettings>(configuration.GetSection("OasisSettings")); // used for IOperationErrorBodyWriterService, OperaErrorBodyWriter

            serviceCollection.AddOptiiAuthentication<ClientFactorySignInService>(); // This is needed for both pms processor and oasis security

            serviceCollection.TryAddSingleton<IResponseMessageService, OasisResponseService>();
            serviceCollection.TryAddSingleton<IOasisSecurityService, OasisSecurityService>();
            serviceCollection.TryAddSingleton<IMessageFilter, LinkControllerOasisMessageFilter>();

            // Authentication
            serviceCollection.AddMemoryCache();
            serviceCollection.TryAddSingleton<ILoginCacheService, OptiiLoginCache>();
            serviceCollection.TryAddSingleton<ICachedExternalLogin, CachedExternalLogin>();

            serviceCollection.TryAddSingleton<IOperationErrorBodyWriterService, OperaErrorBodyWriter>();

            return serviceCollection;
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

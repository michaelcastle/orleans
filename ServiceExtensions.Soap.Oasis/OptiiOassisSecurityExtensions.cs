using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ServiceExtensions.PmsAdapter.PmsProcessor;
using ServiceExtensions.PmsAdapter.SignIn;
using ServiceExtensions.PmsAdapter.SignIn.CachedLogin;
using ServiceExtensions.Soap.Core;
using ServiceExtensions.Soap.Core.Oasis;
using ServiceExtensions.Soap.Core.Response;

namespace ServiceExtensions.Soap.Oasis
{
    public static class OptiiOassisSecurityExtensions
    {
        public static IServiceCollection AddOptiiOasisSecurityFilter<TU>(this IServiceCollection serviceCollection)
            where TU : class, ISessionItemAuthenticationService
        {
            var configuration = serviceCollection.BuildServiceProvider()
                .GetService<IConfiguration>();

            serviceCollection.Configure<OasisSettings>(configuration.GetSection("OasisSettings")); // used for IOperationErrorBodyWriterService, OperaErrorBodyWriter

            serviceCollection.AddOptiiAuthentication<TU>(); // This is needed for both pms processor and oasis security

            serviceCollection.TryAddSingleton<IResponseMessageService, OasisResponseService>();
            serviceCollection.TryAddSingleton<IOasisSecurityService, OasisSecurityService>();
            serviceCollection.TryAddSingleton<IMessageFilter, OasisMessageFilter>();

            // Authentication
            serviceCollection.AddMemoryCache();
            serviceCollection.TryAddSingleton<ILoginCacheService, OptiiLoginCache>();
            serviceCollection.TryAddSingleton<ICachedExternalLogin, CachedExternalLogin>();

            serviceCollection.TryAddSingleton<IOperationErrorBodyWriterService, OperaErrorBodyWriter>();

            return serviceCollection;
        }
    }
}

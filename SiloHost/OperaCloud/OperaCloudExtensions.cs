using LinkController.OperaCloud.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NodaTime;
using OutboundAdapter.Interfaces.StreamHelpers;
using Polly;
using ServiceExtensions.PmsAdapter.PmsProcessor;
using ServiceExtensions.PmsAdapter.SignIn;
using ServiceExtensions.PmsAdapter.SignIn.Authentication;
using ServiceExtensions.PmsAdapter.SignIn.CachedLogin;
using ServiceExtensions.PmsAdapter.SignIn.V2;
using System;
using System.Net.Http;

namespace SiloHost.OperaCloud
{
    public static class OperaCloudExtensions
    {
        public static IServiceCollection AddOperaCloud(this IServiceCollection services)
        {
            //https://www.stevejgordon.co.uk/introduction-to-httpclientfactory-aspnetcore
            var timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(10));
            services.AddHttpClient(nameof(Constants.Outbound.OperaCloud), client =>
            {
                //client.DefaultRequestHeaders.Add("Content-Type", "text/xml");
                //client.DefaultRequestHeaders.Add("SOAPAction", "http://webservices.micros.com/htng/2008B/SingleGuestItinerary#UpdateRoomStatus");
            })
            .AddPolicyHandler(timeoutPolicy)
            .AddTransientHttpErrorPolicy(p => p.RetryAsync(3));

            services.TryAddSingleton<IPmsProcessorService, PmsProcessorService>();
            services.AddMemoryCache();
            services.TryAddSingleton<ILoginCacheService, OptiiLoginCache>();
            services.TryAddTransient<ICachedExternalLogin, CachedExternalLogin>();
            services.TryAddTransient<ISessionItemAuthenticationService, ClientFactorySignInService>();
            services.TryAddTransient<ISecurityAuthenticator, OptiiAuthenticator>();

            services.TryAddTransient<IStreamNamespaces, StreamNamespaces>();

            services.TryAddSingleton<IClock>(SystemClock.Instance);
            services.TryAddTransient<IOperaCloudEnvelopeSerializer, OperaEnvelopeSerializer>();

            return services;
        }
    }
}

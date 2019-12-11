using Microsoft.Extensions.DependencyInjection;
using NodaTime;
using OutboundAdapter.Grains.Opera;
using OutboundAdapter.Interfaces.Opera;
using Polly;
using System;
using System.Net.Http;

namespace SiloHost.Opera
{
    public static class OperaExtensions
    {
        public static IServiceCollection AddOpera(this IServiceCollection services)
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

            services.AddSingleton<IClock>(SystemClock.Instance);
            services.AddTransient<IOperaEnvelopeSerializer, OperaEnvelopeSerializer>();

            return services;
        }
    }
}

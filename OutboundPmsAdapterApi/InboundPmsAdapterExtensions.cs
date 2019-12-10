using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OutboundAdapter.Grains.Opera;
using PmsAdapter.Api.Controllers.Opera;
using ServiceExtensions.PmsAdapter.SubmitMessage;
using ServiceExtensions.Soap.Oasis;

namespace PmsAdapter.Api
{
    public static class InboundPmsAdapterExtensions
    {
        public static IServiceCollection AddInboundPmsAdapter(this IServiceCollection services)
        {
            services.TryAddSingleton<ISubmitMessageHandler, SubmitMessageOrleans>();
            services.TryAddSingleton<InboundController>();
            services.AddOptiiOasisSecurityFilter<InboundPmsAdapterSignInService>();
            return services;
        }
    }
}

using Microsoft.Extensions.DependencyInjection;
using ServiceExtensions.Formatters.BodyRequestFormatters;

namespace PmsAdapter.Api.OperaCloud.Outbound
{
    public static class SerialiserExtensions
    {
        public static IServiceCollection AddMvcRequestSerialisers(this IServiceCollection services)
        {
            services.AddMvc(o => o.InputFormatters.Insert(0, new RawRequestBodyFormatter()))
                   .AddXmlSerializerFormatters()
                   .AddXmlDataContractSerializerFormatters();
            return services;
        }
    }
}

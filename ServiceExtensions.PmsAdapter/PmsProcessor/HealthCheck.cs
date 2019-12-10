using Microsoft.Extensions.Diagnostics.HealthChecks;
using ServiceExtensions.PmsAdapter.ClientChannel;
using System;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using ServiceExtensions.PmsAdapter.Connected_Services.PmsProcessor;

namespace ServiceExtensions.PmsAdapter.PmsProcessor
{
    public class HealthCheck : IHealthCheck
    {
        private readonly IClientChannelFactory<IPMSInterfaceContractChannel> _clientFactory;

        public HealthCheck(IClientChannelFactory<IPMSInterfaceContractChannel> clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            var client = _clientFactory.CreateChannel();

            try
            {
                client.Open();

                var result = await client.PmsAdapterHealthCheckAsync();
                if (result == null)
                {
                    return new HealthCheckResult(context.Registration.FailureStatus, "Health Check did not receive a valid response from PmsProcessor");
                }
                else if (result.ReturnType != InterfaceReturn.enReturnType.Success)
                {
                    return new HealthCheckResult(context.Registration.FailureStatus, $"Health Check ReturnType: [{result.ReturnType}]");
                }

                return HealthCheckResult.Healthy($"{_clientFactory.EndPoint.Address.Uri} is active");
            }
            catch (TimeoutException ex)
            {
                return new HealthCheckResult(context.Registration.FailureStatus, exception: ex);
            }
            catch (CommunicationException ex)
            {
                return new HealthCheckResult(context.Registration.FailureStatus, exception: ex);
            }
            catch (Exception ex)
            {
                return new HealthCheckResult(context.Registration.FailureStatus, exception: ex);
            }
            finally
            {
                client.Close();
            }
        }
    }
}

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using ServiceExtensions.PmsAdapter.PmsProcessor;
using ServiceExtensions.PmsAdapter.SubmitMessage.Direct;
using ServiceExtensions.PmsAdapter.SubmitMessage.ToQueue;
using System;

namespace ServiceExtensions.PmsAdapter.SubmitMessage
{
    public static class SubmitMessageExtensions
    {
        public static IServiceCollection AddSubmitMessageHandler(this IServiceCollection serviceCollection)
        {
            var pmsProcessorSettingsSettings = serviceCollection.BuildServiceProvider()
                 .GetService<IOptions<PmsProcessorSettings>>()
                 .Value ?? new PmsProcessorSettings();

            switch (pmsProcessorSettingsSettings.SubmitMessage.SubmitType)
            {
                case SubmitType.Direct:
                    {
                        serviceCollection.TryAddSingleton<ISubmitMessageHandler, InboundPmsAdapter>();
                        break;
                    }

                default:
                    {
                        serviceCollection.TryAddSingleton<ISubmitMessageHandler, SubmitMessageToQueue>();
                        break;
                    }
            }

            return serviceCollection;
        }

        /// <summary>
        /// The Submit extension when the username and password have been set in the <see cref="PmsProcessorSettings"/> appsettings.json configuration.
        /// <para>
        /// An <see cref="ISubmitMessageHandler"/> extension for submitting a message to the optii v2 pms processor api.
        /// </para>
        /// <para>
        /// The <see cref="IConfiguration"/> required for the credentials and encryption if desired.
        /// </para>
        /// </summary>
        public static SubmitMessageResponse Submit(this ISubmitMessageHandler submitMessageHandler, PmsProcessorSettings pmsProcessorSettings, string body)
        {
            try
            {
                var submitMessage = new SubmitMessage
                {
                    Message = body,
                    Username = pmsProcessorSettings.Credentials?.Username ?? string.Empty,
                    Password = pmsProcessorSettings.Credentials?.Password ?? string.Empty
                };
                return submitMessageHandler.Submit(submitMessage).Result;
            }
            catch (Exception ex)
            {
                return new SubmitMessageResponse
                {
                    IsSuccessful = false,
                    FailReason = ex
                };
            }
        }
    }
}

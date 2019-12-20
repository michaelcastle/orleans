using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ServiceExtensions.Soap.Core.Oasis;
using ServiceExtensions.Soap.Core.Response;
using System;
using System.ServiceModel.Channels;

namespace ServiceExtensions.Soap.Core
{
    public static class SoapEndpointExtensions
    {
        public static IApplicationBuilder UseSoapEndpoint<T>(this IApplicationBuilder builder, string path, MessageEncoder encoder, SoapSerializer serializer = SoapSerializer.DataContractSerializer, bool caseInsensitivePath = false)
        {
            return builder.UseMiddleware<SoapEndpointMiddleware>(typeof(T), path, encoder, serializer, caseInsensitivePath);
        }

        public static IApplicationBuilder UseSoapEndpoint<T>(this IApplicationBuilder builder, string path, Binding binding, SoapSerializer serializer = SoapSerializer.DataContractSerializer, bool caseInsensitivePath = false)
        {
            var element = binding.CreateBindingElements().Find<MessageEncodingBindingElement>();
            var factory = element.CreateMessageEncoderFactory();
            var encoder = factory.Encoder;
            return builder.UseSoapEndpoint<T>(path, encoder, serializer, caseInsensitivePath);
        }

        public static IServiceCollection AddSoapExceptionTransformer(this IServiceCollection serviceCollection, Func<Exception, string> transformer)
        {
            serviceCollection.TryAddSingleton(new ExceptionTransformer(transformer));
            return serviceCollection;
        }

        public static IServiceCollection AddSoapMessageInspector(this IServiceCollection serviceCollection, IMessageInspector messageInspector)
        {
            serviceCollection.TryAddSingleton(messageInspector);
            return serviceCollection;
        }

        public static IServiceCollection AddSoapMessageFilter(this IServiceCollection serviceCollection, IMessageFilter messageFilter)
        {
            serviceCollection.TryAddSingleton(messageFilter);
            return serviceCollection;
        }

        public static IServiceCollection AddSoapWsSecurityFilter(this IServiceCollection serviceCollection, string username, string password)
        {
            serviceCollection.AddSoapMessageFilter(new WsMessageFilter(username, password));
            return serviceCollection;
        }

        public static IServiceCollection AddSoapOasisSecurityFilter(this IServiceCollection serviceCollection)
        {
            //serviceCollection.TryAddSingleton(authenticator);
            serviceCollection.TryAddSingleton<IOasisSecurityService, OasisSecurityService>();
            //serviceCollection.TryAddSingleton<IMessageFilter, OasisMessageFilter>();
            serviceCollection.TryAddSingleton<IResponseMessageService, OasisResponseService>();
            return serviceCollection;
        }

        public static IServiceCollection AddCustomErrorBodyWriter(this IServiceCollection serviceCollection, IOperationErrorBodyWriterService operationErrorBodyWriter)
        {
            serviceCollection.TryAddSingleton(operationErrorBodyWriter);
            return serviceCollection;
        }

        public static IServiceCollection AddSoapModelBindingFilter(this IServiceCollection serviceCollection, IModelBindingFilter modelBindingFilter)
        {
            serviceCollection.TryAddSingleton(modelBindingFilter);
            return serviceCollection;
        }

        public static IServiceCollection AddResponseMessage(this IServiceCollection serviceCollection, IResponseMessageService responseMessage)
        {
            serviceCollection.TryAddSingleton(responseMessage);
            return serviceCollection;
        }
    }
}

using Microsoft.AspNetCore.Http;
using System;
using System.ServiceModel.Channels;

namespace ServiceExtensions.Soap.Core
{
    public interface IMessageFilter
    {
        void OnRequestExecuting(Message message, HttpRequest path);
        void OnResponseExecuting(Message message);
    }
}

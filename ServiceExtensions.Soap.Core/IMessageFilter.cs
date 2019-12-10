using System.ServiceModel.Channels;

namespace ServiceExtensions.Soap.Core
{
    public interface IMessageFilter
    {
        void OnRequestExecuting(Message message);
        void OnResponseExecuting(Message message);
    }
}

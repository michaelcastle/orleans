using System.ServiceModel.Channels;

namespace ServiceExtensions.Soap.Core
{
    public interface IMessageInspector
    {
        object AfterReceiveRequest(ref Message message);
        void BeforeSendReply(ref Message reply, object correlationState);
    }
}

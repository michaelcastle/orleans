using System.ServiceModel.Channels;

namespace ServiceExtensions.Soap.Core.Response
{
    public class OasisResponseService : IResponseMessageService
    {
        public Message GetResponse(MessageHeaders messageHeaders, Message message)
        {
            return new OasisResponseMessage(messageHeaders, message);
        }
    }
}

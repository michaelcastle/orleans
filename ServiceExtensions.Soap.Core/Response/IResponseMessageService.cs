using System.ServiceModel.Channels;

namespace ServiceExtensions.Soap.Core.Response
{
    public interface IResponseMessageService
    {
        Message GetResponse(MessageHeaders messageHeaders, Message message);
    }
}

using System.ServiceModel.Channels;

namespace ServiceExtensions.Soap.Core.Oasis
{
    public interface IOasisSecurityService
    {
        OasisSecurity GetOasisSecurity(MessageHeaders headers);
    }
}

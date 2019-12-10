using System;
using System.ServiceModel.Channels;
using System.Xml.Serialization;

namespace ServiceExtensions.Soap.Core.Oasis
{
    public class OasisSecurityService : IOasisSecurityService
    {
        public OasisSecurity GetOasisSecurity(MessageHeaders headers)
        {
            OasisSecurity oasisSecurity = null;
            var i = headers.FindHeader(OasisConstants.SecurityRootNode, OasisConstants.SecurityNamespace);

            if (i > 0)
            {
                var securityHeader = headers[i].ToString();

                var serializer = new XmlSerializer(typeof(OasisSecurity));
                using (var reader = new System.IO.StringReader(securityHeader))
                {
                    oasisSecurity = (OasisSecurity)serializer.Deserialize(reader);
                }
            }

            if (oasisSecurity == null)
            {
                throw new Exception();
            }

            return oasisSecurity;
        }
    }
}

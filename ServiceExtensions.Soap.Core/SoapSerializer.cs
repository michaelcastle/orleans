namespace ServiceExtensions.Soap.Core
{
    public enum SoapSerializer
    {
        /// <summary>
        /// Client created from wsdl via Connected Services - Add Service Reference (see https://stackoverflow.com/a/2468182)
        /// </summary>
        XmlSerializer,

        /// <summary>
        /// Client created from interface via <see cref="System.ServiceModel.ChannelFactory" />
        /// </summary>
        DataContractSerializer,

        /// <summary>
        /// Client created from wsdl via Connected Services, extracts the body as string and doesn't deserialize Request Message to an object
        /// </summary>
        StringBodyXmlSerializer,

        /// <summary>
        /// Client created from interface via <see cref="System.ServiceModel.ChannelFactory" />, extracts the body as string and doesn't deserialize Request Message to an object
        /// </summary>
        StringBodyDataContractSerializer
    }
}

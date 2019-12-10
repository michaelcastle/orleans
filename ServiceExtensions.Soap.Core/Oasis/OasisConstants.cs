namespace ServiceExtensions.Soap.Core.Oasis
{
    public static class OasisConstants
    {
        // Namespace constants
        public const string SecurityNamespaceAlias = "wsse";
        public const string SecurityNamespace = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd";
        public const string UtilityNamespaceAlias = "wsu";
        public const string UtilityNamespace = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd";

        // XML Node Constants
        public const string SecurityRootNode = "Security";

        // Timestamp Nodes
        public const string TimestampRootNode = "Timestamp";
        public const string TimestampIdAttribute = "Id";
        public const string TimestampCreatedNode = "Created";
        public const string TimestampExpiresNode = "Expires";

        // Username Token Nodes
        public const string UsernameTokenRootNode = "UsernameToken";
        public const string UsernameTokenUsernameNode = "Username";
        public const string UsernameTokenPasswordNode = "Password";
        public const string UsernameTokenNonceNode = "Nonce";
        public const string UsernameTokenCreatedNode = "Created";
    }
}

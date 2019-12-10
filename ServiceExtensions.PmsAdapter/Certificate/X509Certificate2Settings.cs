namespace ServiceExtensions.PmsAdapter.Certificate
{
    public class X509Certificate2Settings
    {
        public string Path { get; set; }
        public string Thumbprint { get; set; }
        public string StoreName { get; set; }
        public string StoreLocation { get; set; }
        public string X509FindType { get; set; }
        public string X509FindValue { get; set; }
        public bool X509ValidOnly { get; set; }
    }
}

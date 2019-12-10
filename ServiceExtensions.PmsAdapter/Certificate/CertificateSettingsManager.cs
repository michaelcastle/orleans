using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace ServiceExtensions.PmsAdapter.Certificate
{
    public class CertificateSettingsManager
    {
        private readonly X509Certificate2Settings _settings;

        public StoreName StoreName {  get
            {
                var storeName = StoreName.Root;
                if (!string.IsNullOrEmpty(_settings.StoreName))
                {
                    Enum.TryParse(_settings.StoreName, out storeName);
                }

                return storeName;
            }
        }

        public StoreLocation StoreLocation
        {
            get
            {
                var storeLocation = StoreLocation.LocalMachine;
                if (!string.IsNullOrEmpty(_settings.StoreLocation))
                {
                    Enum.TryParse(_settings.StoreLocation, out storeLocation);
                }

                return storeLocation;
            }
        }

        public X509FindType X509FindType
        {
            get
            {
                var findType = X509FindType.FindByThumbprint;
                if (!string.IsNullOrEmpty(_settings.X509FindType))
                {
                    Enum.TryParse(_settings.X509FindType, out findType);
                }

                return findType;
            }
        }

        private X509Store GetCertificateStore()
        {
            if (!string.IsNullOrEmpty(_settings.StoreName) && !string.IsNullOrEmpty(_settings.StoreLocation))
            {
                return new X509Store(StoreName, StoreLocation);
            }

            if (!string.IsNullOrEmpty(_settings.StoreName) && string.IsNullOrEmpty(_settings.StoreLocation))
            {
                return new X509Store(StoreName);
            }

            if (string.IsNullOrEmpty(_settings.StoreName) && !string.IsNullOrEmpty(_settings.StoreLocation))
            {
                return new X509Store(StoreLocation);
            }

            return new X509Store();
        }

        public CertificateSettingsManager(X509Certificate2Settings settings)
        {
            _settings = settings;
        }

        public X509Certificate2 GetCertificate()
        {
            if (!string.IsNullOrEmpty(_settings.Thumbprint))
            {
                return GetCertificateByThumbprint(_settings.Thumbprint);
            }

            if (!string.IsNullOrEmpty(_settings.Path))
            {
                return GetCertificateByThumbprint(_settings.Path);
            }

            if (!string.IsNullOrEmpty(_settings.StoreName) && !string.IsNullOrEmpty(_settings.StoreLocation) && !string.IsNullOrEmpty(_settings.X509FindType) && !string.IsNullOrEmpty(_settings.X509FindValue)) {
                return GetCertificateByFind();
            }

            return new X509Certificate2();
        }

        public X509Certificate2 GetCertificateByPath(string path) // reads a pfx from disk
        {
            var certificate = new X509Certificate2(File.ReadAllBytes(path));
            using (var certStore = GetCertificateStore())
            {
                certStore.Open(OpenFlags.ReadWrite);
                var x509Certificate2Collection = certStore.Certificates.Find(X509FindType.FindByThumbprint, certificate.Thumbprint ?? throw new InvalidOperationException("Certificate not found"), false);
                if (x509Certificate2Collection.Count == 0)
                    certStore.Add(certificate);
            }

            return certificate;
        }

        public X509Certificate2 GetCertificateByThumbprint(string thumbprint)
        {
            if (string.IsNullOrEmpty(thumbprint))
            {
                throw new ArgumentException("Certificate thumbprint must not be empty");
            }

            using (var certStore = GetCertificateStore())
            {
                certStore.Open(OpenFlags.ReadOnly);

                var certCollection = certStore.Certificates.Find(
                    findType: X509FindType.FindByThumbprint,
                    findValue: thumbprint,
                    validOnly: false);

                if (certCollection.Count > 0)
                {
                    return certCollection[0];
                }
            }

            throw new ArgumentException($"Certificate store does not contain certificate '{thumbprint}'.");
        }

        public X509Certificate2 GetCertificateByFind()
        {
            using (var certStore = GetCertificateStore())
            {
                certStore.Open(OpenFlags.ReadOnly);

                var findResult = certStore.Certificates.Find(X509FindType, _settings.X509FindValue, _settings.X509ValidOnly);

                return findResult[0];
            }
        }
    }
}

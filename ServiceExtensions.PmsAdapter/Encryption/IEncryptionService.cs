namespace ServiceExtensions.PmsAdapter.Encryption
{
    public interface IEncryptionService
    {
        string Encrypt(string plainText, string sharedSecret, string salt);
    }
}

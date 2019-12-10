namespace ServiceExtensions.PmsAdapter.SignIn
{
    public class EncryptionSettings
    {
        public bool EncryptCredentials { get; set; }
        public string Salt { get; set; }
        public string UsernameSharedSecret { get; set; }
        public string PasswordSharedSecret { get; set; }
    }
}

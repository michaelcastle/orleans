using ServiceExtensions.PmsAdapter.SignIn;
using ServiceExtensions.PmsAdapter.SubmitMessage;

namespace ServiceExtensions.PmsAdapter.PmsProcessor
{
    public class PmsProcessorSettings
    {
        public string Url { get; set; }
        public SubmitMessageSettings SubmitMessage { get; set; }
        public EncryptionSettings Encryption { get; set; }
        public CredentialsSettings Credentials { get; set; }
    }
}

using ServiceExtensions.ServiceBus;

namespace ServiceExtensions.PmsAdapter.SubmitMessage
{
    public class SubmitMessageSettings
    {
        public SubmitType SubmitType { get; set; }
        public QueueSettings QueueSettings { get; set; }
    }

    public enum SubmitType
    {
        Direct,
        ToQueue
    }
}

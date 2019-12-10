using System;

namespace ServiceExtensions.PmsAdapter.SubmitMessage
{
    public class SubmitMessageResponse
    {
        public bool IsSuccessful;
        public Exception FailReason { get; set; }
    }
}

using NServiceBus;
using System;

namespace FUS.File.Messages.SagaMessages.MessageValidation
{
    public class MessageValidatedEvent : IEvent
    {
        public Guid TrackingId { get; set; }
        public bool IsValid { get; set; }
        public string Error { get; set; }
    }
}

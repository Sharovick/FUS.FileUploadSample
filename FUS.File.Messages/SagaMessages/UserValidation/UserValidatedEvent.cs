using NServiceBus;
using System;

namespace FUS.File.Messages.SagaMessages.UserValidation
{
    public class UserValidatedEvent : IEvent
    {
        public Guid TrackingId { get; set; }
        public bool IsValid { get; set; }
    }
}

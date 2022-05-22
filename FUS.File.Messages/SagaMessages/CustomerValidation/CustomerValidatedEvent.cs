using NServiceBus;
using System;

namespace FUS.File.Messages.SagaMessages.CustomerValidation
{
    public class CustomerValidatedEvent : IEvent
    {
        public Guid TrackingId { get; set; }
        public bool IsValid { get; set; }
    }
}

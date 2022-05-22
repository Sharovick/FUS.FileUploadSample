using FUS.Common.Models;
using NServiceBus;
using System;

namespace FUS.File.Messages.SagaMessages.FileValidation
{
    public class FileValidatedEvent : IEvent
    {
        public Guid TrackingId { get; set; }
        public bool IsValid { get; set; }
        public Document File { get; set; }    
    }
}

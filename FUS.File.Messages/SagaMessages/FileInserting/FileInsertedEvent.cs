using NServiceBus;
using System;

namespace FUS.File.Messages.SagaMessages.FileInserting
{
    public class FileInsertedEvent : IEvent
    {
        public Guid TrackingId { get; set; }
        public int FileId { get; set; }
    }
}

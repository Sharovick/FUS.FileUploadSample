using NServiceBus;
using System;
using System.Collections.Generic;

namespace FUS.File.Messages
{
    public class FilesUploadedEvent : IEvent
    {
        public Guid TrackingId { get; set; }
        public int UserId { get; set; }
        public int CustomerId { get; set; }
        public IEnumerable<int> FileIdList { get; set; }
    }
}

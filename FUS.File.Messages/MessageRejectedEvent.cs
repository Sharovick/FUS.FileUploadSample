using FUS.Common.Models;
using NServiceBus;
using System;
using System.Collections.Generic;

namespace FUS.File.Messages
{
    public class MessageRejectedEvent : IEvent
    {
        public Guid TrackingId { get; set; }
        public int UserId { get; set; }
        public int CustomerId { get; set; }
        public IEnumerable<Document> Files { get; set; }
    }
}

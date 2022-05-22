using FUS.Common.Models;
using NServiceBus;
using System;

namespace FUS.File.Messages.SagaMessages.FileInserting
{
    public class SendFileToInsertCommand : ICommand
    {
        public Guid TrackingId { get; set; }
        public int CustomerId { get; set; }
        public Document File { get; set; }
    }
}

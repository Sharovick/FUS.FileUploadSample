using FUS.Common.Models;
using NServiceBus;
using System;
using System.Collections.Generic;

namespace FUS.File.Messages.SagaMessages.MessageValidation
{
    public class SendMessageToValidateCommand : ICommand
    {
        public Guid TrackingId { get; set; }
        public bool IsUserValid { get; set; }
        public bool IsCustomerValid { get; set; }
        public IEnumerable<Document> Files { get; set; }
        public IEnumerable<Document> InvalidFiles { get; set; }
    }
}

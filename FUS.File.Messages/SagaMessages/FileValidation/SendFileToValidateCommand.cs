using FUS.Common.Models;
using NServiceBus;
using System;

namespace FUS.File.Messages.SagaMessages.FileValidation
{
    public class SendFileToValidateCommand : ICommand
    {
        public Guid TrackingId { get; set; }
        public Document File { get; set; }
    }
}

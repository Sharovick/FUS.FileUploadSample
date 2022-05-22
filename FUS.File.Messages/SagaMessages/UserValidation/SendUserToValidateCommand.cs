using NServiceBus;
using System;

namespace FUS.File.Messages.SagaMessages.UserValidation
{
    public class SendUserToValidateCommand : ICommand
    {
        public Guid TrackingId { get; set; }
        public int UserId { get; set; }
    }
}

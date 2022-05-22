using NServiceBus;
using System;

namespace FUS.File.Messages.SagaMessages.CustomerValidation
{
    public class SendCustomerToValidateCommand : ICommand
    {
        public Guid TrackingId { get; set; }
        public int CustomerId { get; set; }
    }
}

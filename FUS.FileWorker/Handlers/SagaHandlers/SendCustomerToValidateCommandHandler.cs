using FUS.File.Messages.SagaMessages.CustomerValidation;
using FUS.File.Messages.SagaMessages.UserValidation;
using FUS.Infrastrucure.Interfaces;
using NServiceBus;
using System;
using System.Threading.Tasks;

namespace FUS.File.Worker.Handlers.SagaHandlers
{
    public class SendCustomerToValidateCommandHandler : IHandleMessages<SendCustomerToValidateCommand>
    {
        private readonly ICustomerService _customerService;
        public SendCustomerToValidateCommandHandler(ICustomerService customerService)
        {
            _customerService = customerService;
        }
        public async Task Handle(SendCustomerToValidateCommand message, IMessageHandlerContext context)
        {
            var customerExists = await _customerService.CheckIfCustomerExistsAndActive(message.CustomerId);
            var customerValidatedEvent = new CustomerValidatedEvent { TrackingId = message.TrackingId, IsValid = customerExists };
            Console.WriteLine($"Customer validated: {nameof(message.TrackingId)}: {message.TrackingId}, IsValid: {customerExists}");
            await context.Publish(customerValidatedEvent);
        }
    }
}

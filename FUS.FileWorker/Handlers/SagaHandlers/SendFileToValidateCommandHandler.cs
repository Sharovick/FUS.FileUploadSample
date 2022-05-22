using FUS.Common.Models;
using FUS.File.Messages.SagaMessages.FileValidation;
using NServiceBus;
using System;
using System.Threading.Tasks;

namespace FUS.File.Worker.Handlers.SagaHandlers
{
    public class SendFileToValidateCommandHandler : IHandleMessages<SendFileToValidateCommand>
    {
        public async Task Handle(SendFileToValidateCommand message, IMessageHandlerContext context)
        {
            var fileValidatedEvent = new FileValidatedEvent
            {
                TrackingId = message.TrackingId,
                IsValid = CheckFile(message.File),
                File = message.File
            };
            Console.WriteLine($"File validated: {nameof(message.TrackingId)}: {message.TrackingId}, IsValid: {fileValidatedEvent.IsValid}");
            await context.Publish(fileValidatedEvent);
        }

        private bool CheckFile(Document file)
        {
            // ToDo: Some custom logic on file validation
            return true;
        }
    }
}

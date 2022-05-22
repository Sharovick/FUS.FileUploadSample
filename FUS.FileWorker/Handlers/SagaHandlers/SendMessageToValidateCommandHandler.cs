using FUS.Common.Enums;
using FUS.Common.Models;
using FUS.File.Messages.SagaMessages.MessageValidation;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FUS.File.Worker.Handlers.SagaHandlers
{
    public class SendMessageToValidateCommandHandler : IHandleMessages<SendMessageToValidateCommand>
    {
        public async Task Handle(SendMessageToValidateCommand message, IMessageHandlerContext context)
        {
            var messageValidatedEvent = new MessageValidatedEvent
            {
                TrackingId = message.TrackingId,
                Error = "",
                IsValid = true
            };
            if (!message.IsUserValid)
            {
                messageValidatedEvent.IsValid = false;
                messageValidatedEvent.Error += " User is invalid";
            }
            if (!message.IsCustomerValid)
            {
                messageValidatedEvent.IsValid = false;
                messageValidatedEvent.Error += " Customer is invalid";
            }
            if (!CheckIfAllFilesWhereSent(message.Files))
            {
                messageValidatedEvent.IsValid = false;
                messageValidatedEvent.Error += " There is not enougth files in list";
            }
            if (message.InvalidFiles.Count() > 0)
            {
                messageValidatedEvent.IsValid = false;
                messageValidatedEvent.Error += " There are invalid files in list";
            }
            Console.WriteLine($"Message validated: {nameof(message.TrackingId)}: {message.TrackingId}, IsValid: {messageValidatedEvent.IsValid}, Error: {messageValidatedEvent.Error}");
            await context.Publish(messageValidatedEvent);
        }

        private bool CheckIfAllFilesWhereSent(IEnumerable<Document> files)
        {
            var passportExists = false;
            var gdprBaseExists = false;
            var gdprAnexOneExists = false;
            var partnershipExists = false;
            var policyExists = false;
            var untrackingDocumentExists = false;

            foreach (var file in files)
            {
                switch (file.Type)
                {
                    case FileTypeEnum.PassportScan:
                        passportExists = true;
                        break;
                    case FileTypeEnum.GdprBaseAgreement:
                        gdprBaseExists = true;
                        break;
                    case FileTypeEnum.GdprAnexOneAgreement:
                        gdprAnexOneExists = true;
                        break;
                    case FileTypeEnum.PartnershipAgreement:
                        partnershipExists = true;
                        break;
                    case FileTypeEnum.PolicyDocument:
                        policyExists = true;
                        break;
                    default:
                        untrackingDocumentExists = true;
                        break;
                }
            }
            return passportExists && gdprBaseExists && gdprAnexOneExists && partnershipExists && policyExists && !untrackingDocumentExists;
        }
    }
}

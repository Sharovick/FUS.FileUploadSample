using FUS.File.Messages.SagaMessages.FileInserting;
using FUS.Infrastrucure.Interfaces;
using NServiceBus;
using System;
using System.Threading.Tasks;

namespace FUS.File.Worker.Handlers.SagaHandlers
{
    public class SendFileToInsertCommandHandler : IHandleMessages<SendFileToInsertCommand>
    {
        private readonly IFileService _fileService;
        public SendFileToInsertCommandHandler(IFileService fileService)
        {
            _fileService = fileService;
        }
        public async Task Handle(SendFileToInsertCommand message, IMessageHandlerContext context)
        {
            var id = await _fileService.InserFile(message.CustomerId, message.File);
            var fileInsertedEvent = new FileInsertedEvent
            {
                TrackingId = message.TrackingId,
                FileId = id
            };
            Console.WriteLine($"File inserted: {nameof(message.TrackingId)}: {message.TrackingId}, FileId: {id}");
            await context.Publish(fileInsertedEvent);
        }
    }
}

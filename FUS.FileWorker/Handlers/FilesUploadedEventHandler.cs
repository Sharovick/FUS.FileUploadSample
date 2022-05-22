using FUS.File.Messages;
using NServiceBus;
using System;
using System.Threading.Tasks;

namespace FUS.File.Worker.Handlers
{
    public class FilesUploadedEventHandler : IHandleMessages<FilesUploadedEvent>
    {
        public async Task Handle(FilesUploadedEvent message, IMessageHandlerContext context)
        {
            // ToDo: Send successfull email via SMTP with information about uploaded files
            Console.WriteLine("Success");
        }
    }
}

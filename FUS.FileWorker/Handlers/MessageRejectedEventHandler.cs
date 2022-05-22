using FUS.File.Messages;
using NServiceBus;
using System;
using System.Threading.Tasks;

namespace FUS.File.Worker.Handlers
{
    public class MessageRejectedEventHandler : IHandleMessages<MessageRejectedEvent>
    {
        public async Task Handle(MessageRejectedEvent message, IMessageHandlerContext context)
        {
            // ToDo: Send rejection email via SMTP with information about uploaded files
            Console.WriteLine("Rejected");
        }
    }
}

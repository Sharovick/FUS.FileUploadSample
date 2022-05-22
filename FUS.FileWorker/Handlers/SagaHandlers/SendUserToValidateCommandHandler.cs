using FUS.File.Messages.SagaMessages.UserValidation;
using FUS.Infrastrucure.Interfaces;
using NServiceBus;
using System;
using System.Threading.Tasks;

namespace FUS.File.Worker.Handlers.SagaHandlers
{
    public class SendUserToValidateCommandHandler : IHandleMessages<SendUserToValidateCommand>
    {
        private readonly IUserService _userService;
        public SendUserToValidateCommandHandler(IUserService userService)
        {
            _userService = userService;
        }
        public async Task Handle(SendUserToValidateCommand message, IMessageHandlerContext context)
        {
            var userExists = await _userService.CheckIfUserExistsAndActive(message.UserId);
            var userValidatedEvent = new UserValidatedEvent { TrackingId = message.TrackingId, IsValid = userExists };
            Console.WriteLine($"User validated: {nameof(message.TrackingId)}: {message.TrackingId}, IsValid: {userExists}");
            await context.Publish(userValidatedEvent);
        }
    }
}

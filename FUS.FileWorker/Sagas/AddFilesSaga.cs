using FUS.Common.Models;
using FUS.File.Messages;
using FUS.File.Messages.SagaMessages.CustomerValidation;
using FUS.File.Messages.SagaMessages.FileInserting;
using FUS.File.Messages.SagaMessages.FileValidation;
using FUS.File.Messages.SagaMessages.MessageValidation;
using FUS.File.Messages.SagaMessages.UserValidation;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FUS.File.Worker.Sagas
{
    public class AddFilesSaga : Saga<AddFilesSaga.AddFilesData>,
            IAmStartedByMessages<SendFilesCommand>,
            IHandleMessages<UserValidatedEvent>,
            IHandleMessages<CustomerValidatedEvent>,
            IHandleMessages<FileValidatedEvent>,
            IHandleMessages<FileInsertedEvent>,
            IHandleMessages<MessageValidatedEvent>

    {
        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<AddFilesData> mapper)
        {
            mapper.MapSaga(saga => saga.TrackingId)
                .ToMessage<SendFilesCommand>(message => message.TrackingId)
                .ToMessage<UserValidatedEvent>(message => message.TrackingId)
                .ToMessage<CustomerValidatedEvent>(message => message.TrackingId)
                .ToMessage<FileValidatedEvent>(message => message.TrackingId)
                .ToMessage<FileInsertedEvent>(message => message.TrackingId)
                .ToMessage<MessageValidatedEvent>(message => message.TrackingId);
        }

        public class AddFilesData : ContainSagaData
        {
            public Guid TrackingId { get; set; }
            public int UserId { get; set; }
            public int CustomerId { get; set; }
            public IEnumerable<Document> Files { get; set; }

            public bool IsUserValid { get; set; }
            public bool IsCustomerValid { get; set; }
            public int ValidatedFilesCount { get; set; }
            public List<Document> InvalidFiles { get; set; } = new List<Document>();
            public int InsertedFilesCount { get; set; }
            public List<int> InsertedFileIdsList { get; set; } = new List<int>();
        }
        public async Task Handle(SendFilesCommand message, IMessageHandlerContext context)
        {
            Data.TrackingId = message.TrackingId;
            Data.UserId = message.UserId;
            Data.CustomerId = message.CustomerId;
            Data.Files = message.Files ?? new List<Document>();

            var validateUserCommand = new SendUserToValidateCommand
            {
                TrackingId = Data.TrackingId,
                UserId = Data.UserId
            };
            await context.SendLocal(validateUserCommand);
        }

        public async Task Handle(UserValidatedEvent message, IMessageHandlerContext context)
        {
            Data.IsUserValid = message.IsValid;
            var validateUserCommand = new SendCustomerToValidateCommand
            {
                TrackingId = Data.TrackingId,
                CustomerId = Data.CustomerId
            };
            await context.SendLocal(validateUserCommand);
        }

        public async Task Handle(CustomerValidatedEvent message, IMessageHandlerContext context)
        {
            Data.IsCustomerValid = message.IsValid;
            Data.ValidatedFilesCount = 0;
            foreach (var file in Data.Files)
            {
                var validateUserCommand = new SendFileToValidateCommand
                {
                    TrackingId = Data.TrackingId,
                    File = file
                };
                await context.SendLocal(validateUserCommand);
            }
        }

        public async Task Handle(FileValidatedEvent message, IMessageHandlerContext context)
        {
            Data.ValidatedFilesCount++;
            if (!message.IsValid)
            {
                Data.InvalidFiles.Add(message.File);
            }
            if (Data.ValidatedFilesCount == Data.Files.Count())
            {
                await context.SendLocal(new SendMessageToValidateCommand
                {
                    TrackingId = Data.TrackingId,
                    IsUserValid = Data.IsUserValid,
                    IsCustomerValid = Data.IsCustomerValid,
                    Files = Data.Files,
                    InvalidFiles = Data.InvalidFiles
                });
            }
        }

        public async Task Handle(FileInsertedEvent message, IMessageHandlerContext context)
        {
            Data.InsertedFilesCount++;
            Data.InsertedFileIdsList.Add(message.FileId);
            if (Data.InsertedFilesCount == Data.Files.Count())
            {
                var filesUploadedEvent = new FilesUploadedEvent
                {
                    TrackingId = Data.TrackingId,
                    UserId = Data.UserId,
                    CustomerId = Data.CustomerId,
                    FileIdList = Data.InsertedFileIdsList
                };
                await context.Publish(filesUploadedEvent);
                MarkAsComplete();
            }
        }

        public async Task Handle(MessageValidatedEvent message, IMessageHandlerContext context)
        {
            if (message.IsValid)
            {
                foreach (var file in Data.Files)
                {
                    await context.SendLocal(new SendFileToInsertCommand
                    {
                        TrackingId = Data.TrackingId,
                        CustomerId = Data.CustomerId,
                        File = file
                    });
                }                
            }
            else
            {
                await context.Publish(new MessageRejectedEvent
                {
                    TrackingId = Data.TrackingId,
                    UserId = Data.UserId,
                    CustomerId = Data.CustomerId,
                    Files = Data.Files
                });
                MarkAsComplete();
            }
        }
    }
}

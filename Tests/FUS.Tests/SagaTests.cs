using FUS.Common.Enums;
using FUS.Common.Models;
using FUS.File.Messages;
using FUS.File.Messages.SagaMessages.CustomerValidation;
using FUS.File.Messages.SagaMessages.FileInserting;
using FUS.File.Messages.SagaMessages.FileValidation;
using FUS.File.Messages.SagaMessages.MessageValidation;
using FUS.File.Messages.SagaMessages.UserValidation;
using FUS.File.Worker.Handlers;
using FUS.File.Worker.Handlers.SagaHandlers;
using FUS.File.Worker.Sagas;
using FUS.Infrastrucure.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NServiceBus.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FUS.Tests
{
    [TestClass]
    public class SagaTests
    {
        private AddFilesSaga _saga;
        private SendUserToValidateCommandHandler _sendUserToValidateCommandHandler;
        private SendCustomerToValidateCommandHandler _sendCustomerToValidateCommandHandler;
        private SendFileToValidateCommandHandler _sendFileToValidateCommandHandler;
        private SendMessageToValidateCommandHandler _sendMessageToValidateCommandHandler;
        private SendFileToInsertCommandHandler _sendFileToInsertCommandHandler;
        
        public TestableMessageHandlerContext _context = new TestableMessageHandlerContext();

        [TestInitialize]
        public void Init()
        {
            _saga = new AddFilesSaga() { Data = new AddFilesSaga.AddFilesData { } };

            var userService = new Mock<IUserService>();
            userService.Setup(x => x.CheckIfUserExistsAndActive(It.IsAny<int>())).ReturnsAsync(true);
            _sendUserToValidateCommandHandler = new SendUserToValidateCommandHandler(userService.Object);

            var customerService = new Mock<ICustomerService>();
            customerService.Setup(x => x.CheckIfCustomerExistsAndActive(It.IsAny<int>())).ReturnsAsync(true);
            _sendCustomerToValidateCommandHandler = new SendCustomerToValidateCommandHandler(customerService.Object);

            _sendFileToValidateCommandHandler = new SendFileToValidateCommandHandler();
            _sendMessageToValidateCommandHandler = new SendMessageToValidateCommandHandler();

            var fileService = new Mock<IFileService>();
            fileService.Setup(x => x.InserFile(It.IsAny<int>(), It.IsAny<Document>())).ReturnsAsync(1);
            _sendFileToInsertCommandHandler = new SendFileToInsertCommandHandler(fileService.Object);
        }

        [TestMethod]
        public async Task HandleMessage_Success()
        {
            //Arrange
            var message = new SendFilesCommand
            {
                TrackingId = Guid.NewGuid(),
                UserId = 1,
                CustomerId = 1,
                Files = new List<Document>
                {
                    new Document
                    {
                        Type = FileTypeEnum.PassportScan,
                        FilePath = "filePath"
                    },
                    new Document
                    {
                        Type = FileTypeEnum.GdprBaseAgreement,
                        FilePath = "filePath"
                    },new Document
                    {
                        Type = FileTypeEnum.GdprAnexOneAgreement,
                        FilePath = "filePath"
                    },
                    new Document
                    {
                        Type = FileTypeEnum.PartnershipAgreement,
                        FilePath = "filePath"
                    },
                    new Document
                    {
                        Type = FileTypeEnum.PolicyDocument,
                        FilePath = "filePath"
                    }
                }
            };

            //Act
            await _saga.Handle(message, _context).ConfigureAwait(false);

            //Assert
            var publishedMessage = _context.SentMessages.LastOrDefault().Message;
            Assert.IsInstanceOfType(publishedMessage, typeof(SendUserToValidateCommand));
            // Validate User
            await _sendUserToValidateCommandHandler.Handle((SendUserToValidateCommand)publishedMessage, _context).ConfigureAwait(false);
            publishedMessage = _context.PublishedMessages.LastOrDefault().Message;
            Assert.IsInstanceOfType(publishedMessage, typeof(UserValidatedEvent));

            await _saga.Handle((UserValidatedEvent)publishedMessage, _context).ConfigureAwait(false);
            publishedMessage = _context.SentMessages.LastOrDefault().Message;
            Assert.IsInstanceOfType(publishedMessage, typeof(SendCustomerToValidateCommand));
            // Validate Customer
            await _sendCustomerToValidateCommandHandler.Handle((SendCustomerToValidateCommand)publishedMessage, _context).ConfigureAwait(false);
            publishedMessage = _context.PublishedMessages.LastOrDefault().Message;
            Assert.IsInstanceOfType(publishedMessage, typeof(CustomerValidatedEvent));

            await _saga.Handle((CustomerValidatedEvent)publishedMessage, _context).ConfigureAwait(false);
            publishedMessage = _context.SentMessages.LastOrDefault().Message;
            Assert.IsInstanceOfType(publishedMessage, typeof(SendFileToValidateCommand));
            // Validate File
            await _sendFileToValidateCommandHandler.Handle((SendFileToValidateCommand)publishedMessage, _context).ConfigureAwait(false);
            publishedMessage = _context.PublishedMessages.LastOrDefault().Message;
            Assert.IsInstanceOfType(publishedMessage, typeof(FileValidatedEvent));

            foreach (var file in message.Files)
            {
                await _saga.Handle((FileValidatedEvent)publishedMessage, _context).ConfigureAwait(false);
            }
            publishedMessage = _context.SentMessages.LastOrDefault().Message;
            Assert.IsInstanceOfType(publishedMessage, typeof(SendMessageToValidateCommand));
            // Validate message
            await _sendMessageToValidateCommandHandler.Handle((SendMessageToValidateCommand)publishedMessage, _context).ConfigureAwait(false);
            publishedMessage = _context.PublishedMessages.LastOrDefault().Message;
            Assert.IsInstanceOfType(publishedMessage, typeof(MessageValidatedEvent));

            await _saga.Handle((MessageValidatedEvent)publishedMessage, _context).ConfigureAwait(false);
            publishedMessage = _context.SentMessages.LastOrDefault().Message;
            Assert.IsInstanceOfType(publishedMessage, typeof(SendFileToInsertCommand));
            // Insert files
            await _sendFileToInsertCommandHandler.Handle((SendFileToInsertCommand)publishedMessage, _context).ConfigureAwait(false);
            publishedMessage = _context.PublishedMessages.LastOrDefault().Message;
            Assert.IsInstanceOfType(publishedMessage, typeof(FileInsertedEvent));
            foreach (var file in message.Files)
            {
                await _saga.Handle((FileInsertedEvent)publishedMessage, _context).ConfigureAwait(false);
            }
            publishedMessage = _context.PublishedMessages.LastOrDefault().Message;
            Assert.IsInstanceOfType(publishedMessage, typeof(FilesUploadedEvent));
        }
    }
}

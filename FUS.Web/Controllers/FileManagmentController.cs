using FUS.Common.Models;
using FUS.File.Messages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FUS.FileUploadSample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileManagmentController : ControllerBase
    {
        private readonly IMessageSession _messageSession;
        private readonly ILogger<FileManagmentController> _logger;

        public FileManagmentController(ILogger<FileManagmentController> logger, IMessageSession messageSession)
        {
            _logger = logger;
            _messageSession = messageSession;
        }

        [HttpPost("{userId}/{customerId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostFile(int userId, int customerId, [FromBody] IEnumerable<Document> files)
        {
            _logger.LogInformation($"Revieved {nameof(PostFile)} with params: {nameof(userId)} = {userId}, {nameof(customerId)} = {customerId}");
            _logger.LogInformation("Revieved list of files {files}", files);
            var trackingId = Guid.NewGuid();
            try
            {
                var fusSendFileCommand = new SendFilesCommand
                {
                    TrackingId = trackingId,
                    UserId = userId,
                    CustomerId = customerId,
                    Files = files
                };
                await _messageSession.Send(fusSendFileCommand);
                // Check User on existance
                // Check User on permissions

                // Check customer on existance
                // check customer on files recieved

                // Check files on existance (duplication)

            } catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return Ok(trackingId);
        }

    }
}

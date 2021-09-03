using ER.Application.Common;
using ER.Application.Mediators.Command;
using ER.Application.Models.Email;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PM.IY.EmailRouterDemoApp.Controllers
{
    public class EmailController : APIBaseController
    {
        private readonly AppSettings _appSettings;
        private readonly ILogger<EmailController> _logger;

        public EmailController(AppSettings appSettings, ILogger<EmailController> logger)
        {
            _appSettings = appSettings;
            _logger = logger;
        }


        [HttpPost]
        [Route("Single")]
        public async Task<ActionResult> SingleEmail([FromBody] EmailMessage message)
        {
            _logger.LogDebug($"Received Single Email Request [{message.From}]");

            try
            {
                var headers = GetRequestHeaders();
                //headers.Add(Constants.PM_SERVER_TOKEN, "some token");
                var emailRequest = new EmailMessageRequest()
                {
                    Message = message,
                    MessageMetaData = new MessageMetaData()
                    {   
                        RequestHTTPHeaders = headers
                    }
                };

                var command = new IngestEmailMessageCommand
                {
                    EmailRequestMessage = emailRequest
                };

                var res = await Mediator.Send(command);

                return res.IsSuccessful ? Ok(res) : BadRequest(res);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while processing SingleEmail Request : [{ex.Message}]");
                return Problem($"Error while processing SingleEmail Request : [{ex.Message}]");
            }
        }

        [HttpPost]
        [Route("Batch")]
        public async Task<ActionResult> BatchEmail([FromBody] EmailMessage message)
        {
            _logger.LogDebug($"Received Batch EMail Request [{message.From}]");

            try
            {
                var headers = GetRequestHeaders();
                //headers.Add(Constants.PM_SERVER_TOKEN, "some token");
                var emailRequest = new EmailMessageRequest()
                {
                    Message = message,
                    MessageMetaData = new MessageMetaData()
                    {
                        RequestHTTPHeaders = headers
                    }
                };

                var command = new IngestEmailMessageCommand
                {
                    EmailRequestMessage = emailRequest
                };

                var res = await Mediator.Send(command);

                return res.IsSuccessful ? Ok(res) : BadRequest(res);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while processing BatchEmail Request : [{ex.Message}]");
                return Problem($"Error while processing BatchEmail Request : [{ex.Message}]");
            }
        }
    }
}

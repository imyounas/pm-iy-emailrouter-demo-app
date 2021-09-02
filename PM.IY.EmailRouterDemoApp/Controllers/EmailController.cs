using ER.Application.Common;
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
            _logger.LogDebug($"Received Single EMail Request [{message.From}]");

            try
            {

                return Ok();
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
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while processing BatchEmail Request : [{ex.Message}]");
                return Problem($"Error while processing BatchEmail Request : [{ex.Message}]");
            }
        }
    }
}

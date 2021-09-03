using ER.Application.Models.Email;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ER.Tests
{
  
    public class APIIntegrationTest : IClassFixture<WebApplicationFactory<PM.IY.EmailRouterDemoApp.Startup>>
    {
        private readonly WebApplicationFactory<PM.IY.EmailRouterDemoApp.Startup> _factory;

        public APIIntegrationTest(WebApplicationFactory<PM.IY.EmailRouterDemoApp.Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task SingleEmailAPITest_Passing()
        {
            var client = _factory.CreateClient();
            var singleEmailRequest = new EmailMessage()
            {
                From = "imran@gmail.com",
                To = "abc@yahoo.com,xyz@yahoo.com,aaa@zinga.com",
                Subject = "Welcome to Wildbit",
                HtmlBody = "<h> Welcome Imran </h1>",
                ReplyTo = "imran@gmail.com",
                TrackOpens = true,
                TrackLinks = ETrackLinks.HtmlAndText.ToString(),
                Attachments = new List<EmailAttachment>()
                { new EmailAttachment() { Id = Guid.NewGuid(), Name = "readme.txt" , ContentType = "text/plain", Content = "dGVzdCBjb250ZW50" } },

            };

            client.DefaultRequestHeaders.Add(Application.Common.Constants.PM_SERVER_TOKEN, "Some-PM-Token-xyz-876");
            client.DefaultRequestHeaders.Add(Application.Common.Constants.ACCEPT_HEADER, "application/json");

            var response = await client.PostAsJsonAsync("/api/Email/Single", singleEmailRequest);

            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Contains("application/json", response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task SingleEmailAPITest_Validation_Failed()
        {
            var client = _factory.CreateClient();
            var singleEmailRequest = new EmailMessage()
            {
                From = "imran@gmail.com",
                To = "abc@yahoo.com,xyz@yahoo.com,aaazinga.com",
                Subject = "Welcome to Wildbit",
                HtmlBody = "",
                ReplyTo = "imran@gmail.com",
                TrackOpens = true,
                TrackLinks = ETrackLinks.HtmlAndText.ToString(),
                Attachments = new List<EmailAttachment>()
                { new EmailAttachment() { Id = Guid.NewGuid(), Name = "readme.txt" , ContentType = "text/plain", Content = "dGVzdCBjb250ZW50" } },

            };

            client.DefaultRequestHeaders.Add(Application.Common.Constants.PM_SERVER_TOKEN, "Some-PM-Token-xyz-876");
            client.DefaultRequestHeaders.Add(Application.Common.Constants.ACCEPT_HEADER, "application/json");

            var response = await client.PostAsJsonAsync("/api/Email/Single", singleEmailRequest);

            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Contains("application/json", response.Content.Headers.ContentType.ToString());
        }
    }
}

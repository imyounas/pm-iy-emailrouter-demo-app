using ER.Application.Common;
using ER.Application.Models.Email;
using ER.Application.Validators.Email;
using FluentValidation.TestHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ER.Tests
{
   
    public class EmailValidationTest
    {
        EmailMessagetValidator emailValidator;
        public EmailValidationTest()
        {
            emailValidator = new EmailMessagetValidator();
        }

        [Fact]
        public void EmailMessageShouldPass()
        {
            var singleEmailRequest = new EmailMessage()
            {
                From = "imran@gmail.com",
                To = "1@yahoo.com",
                Subject = "Welcome to Wildbit",
                HtmlBody = "<h> Welcome Imran </h1>",
                ReplyTo = "imran@gmail.com",
                TrackOpens = true,
                TrackLinks = ETrackLinks.HtmlAndText.ToString(),
                Attachments = new List<EmailAttachment>()
                { new EmailAttachment() { Id = Guid.NewGuid(), Name = "readme.txt" , ContentType = "text/plain", Content = "dGVzdCBjb250ZW50" } },

            };

            var result = emailValidator.TestValidate(singleEmailRequest);
            Assert.True(result.IsValid);
        }

        [Fact]
        public void EmailMessageShouldFailDueToInvalidSeparatorInToAddress()
        {
            var singleEmailRequest = new EmailMessage()
            {
                From = "imran@gmail.com",
                To = "1@yahoo.com;2@yahoo.com",
                Subject = "Welcome to Wildbit",
                HtmlBody = "<h> Welcome Imran </h1>",
                ReplyTo = "imran@gmail.com",
                TrackOpens = true,
                TrackLinks = ETrackLinks.HtmlAndText.ToString(),
                Attachments = new List<EmailAttachment>()
                { new EmailAttachment() { Id = Guid.NewGuid(), Name = "readme.txt" , ContentType = "text/plain", Content = "dGVzdCBjb250ZW50" } },

            };

            var result = emailValidator.TestValidate(singleEmailRequest);
            Assert.False(result.IsValid);
            Assert.Contains(Constants.INVALID_TO, result.Errors.Select(x => x.ErrorCode));
        }

        [Fact]
        public void EmailMessageShouldFailDueToBothBodiesMissing()
        {
            var singleEmailRequest = new EmailMessage()
            {
                From = "imran@gmail.com",
                To = "abc@yahoo.com,xyz@yahoo.com,aaa@zinga.com",
                Subject = "Welcome to Wildbit",
                ReplyTo = "imran@gmail.com",
                TrackOpens = true,
                TrackLinks = ETrackLinks.HtmlAndText.ToString(),
                Attachments = new List<EmailAttachment>()
                { new EmailAttachment() { Id = Guid.NewGuid(), Name = "readme.txt" , ContentType = "text/plain", Content = "dGVzdCBjb250ZW50" } },

            };

            var result = emailValidator.TestValidate(singleEmailRequest);
            Assert.False(result.IsValid);
            Assert.Contains(Constants.INVALID_HTML_OR_TEXT_BODY, result.Errors.Select(x => x.ErrorCode));
        }

        [Fact]
        public void EmailMessageShouldFailDueToInvalidFromAddress()
        {
            var singleEmailRequest = new EmailMessage()
            {
                From = "imrangmail.com",
                To = "abc@yahoo.com,xyz@yahoo.com,aaa@zinga.com",
                Subject = "Welcome to Wildbit",
                ReplyTo = "imran@gmail.com",
                HtmlBody = "<h> Welcome Imran </h1>",
                TrackOpens = true,
                TrackLinks = ETrackLinks.HtmlAndText.ToString(),
                Attachments = new List<EmailAttachment>()
                { new EmailAttachment() { Id = Guid.NewGuid(), Name = "readme.txt" , ContentType = "text/plain", Content = "dGVzdCBjb250ZW50" } },

            };

            var result = emailValidator.TestValidate(singleEmailRequest);
            Assert.False(result.IsValid);
            Assert.Contains(Constants.INVALID_FROM, result.Errors.Select(x => x.ErrorCode));
        }

        [Fact]
        public void EmailMessageShouldFailDueToBlankFromAddress()
        {
            var singleEmailRequest = new EmailMessage()
            {
                From = "",
                To = "abc@yahoo.com,xyz@yahoo.com,aaa@zinga.com",
                Subject = "Welcome to Wildbit",
                ReplyTo = "imran@gmail.com",
                HtmlBody = "<h> Welcome Imran </h1>",
                TrackOpens = true,
                TrackLinks = ETrackLinks.HtmlAndText.ToString(),
                Attachments = new List<EmailAttachment>()
                { new EmailAttachment() { Id = Guid.NewGuid(), Name = "readme.txt" , ContentType = "text/plain", Content = "dGVzdCBjb250ZW50" } },

            };

            var result = emailValidator.TestValidate(singleEmailRequest);
            Assert.False(result.IsValid);
            Assert.Contains(Constants.INVALID_FROM, result.Errors.Select(x => x.ErrorCode));
        }

        [Fact]
        public void EmailMessageShouldFailDueToInvalidToAddress()
        {
            var singleEmailRequest = new EmailMessage()
            {
                From = "imran@gmail.com",
                To = "",
                Subject = "Welcome to Wildbit",
                ReplyTo = "imran@gmail.com",
                HtmlBody = "<h> Welcome Imran </h1>",
                TrackOpens = true,
                TrackLinks = ETrackLinks.HtmlAndText.ToString(),
                Attachments = new List<EmailAttachment>()
                { new EmailAttachment() { Id = Guid.NewGuid(), Name = "readme.txt" , ContentType = "text/plain", Content = "dGVzdCBjb250ZW50" } },

            };

            var result = emailValidator.TestValidate(singleEmailRequest);
            Assert.False(result.IsValid);
            Assert.Contains(Constants.INVALID_TO, result.Errors.Select(x => x.ErrorCode));
        }


        [Fact]
        public void EmailMessageShouldFailDueToMoreThan50ToAddress()
        {
            var singleEmailRequest = new EmailMessage()
            {
                From = "imran@gmail.com",
                To = "aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com",
                Subject = "Welcome to Wildbit",
                ReplyTo = "imran@gmail.com",
                HtmlBody = "<h> Welcome Imran </h1>",
                TrackOpens = true,
                TrackLinks = ETrackLinks.HtmlAndText.ToString(),
                Attachments = new List<EmailAttachment>()
                { new EmailAttachment() { Id = Guid.NewGuid(), Name = "readme.txt" , ContentType = "text/plain", Content = "dGVzdCBjb250ZW50" } },

            };

            var result = emailValidator.TestValidate(singleEmailRequest);
            Assert.False(result.IsValid);
            Assert.Contains(Constants.MAX_TO, result.Errors.Select(x => x.ErrorCode));
        }

        [Fact]
        public void EmailMessageShouldFailDueToMoreThan50BCCAddress()
        {
            var singleEmailRequest = new EmailMessage()
            {
                From = "imran@gmail.com",
                To = "jazz@gmail.com",
                Bcc = "aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com,aaa@gmail.com",
                Subject = "Welcome to Wildbit",
                ReplyTo = "imran@gmail.com",
                HtmlBody = "<h> Welcome Imran </h1>",
                TrackOpens = true,
                TrackLinks = ETrackLinks.HtmlAndText.ToString(),
                Attachments = new List<EmailAttachment>()
                { new EmailAttachment() { Id = Guid.NewGuid(), Name = "readme.txt" , ContentType = "text/plain", Content = "dGVzdCBjb250ZW50" } },

            };

            var result = emailValidator.TestValidate(singleEmailRequest);
            Assert.False(result.IsValid);
            Assert.Contains(Constants.MAX_BCC, result.Errors.Select(x => x.ErrorCode));
        }
    }
}

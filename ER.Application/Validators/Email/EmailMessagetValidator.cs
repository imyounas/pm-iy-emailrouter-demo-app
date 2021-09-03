using ER.Application.Models.Email;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ER.Application.Validators.Email
{
    public class EmailMessagetValidator : AbstractValidator<EmailMessage>
    {
        private Regex _emailRegex;
        public EmailMessagetValidator()
        {
            string validEmailPattern = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";
            _emailRegex = new Regex(validEmailPattern, RegexOptions.IgnoreCase);

            RuleFor(e => e.From).EmailAddress().WithErrorCode(Common.Constants.INVALID_FROM);
            RuleFor(x => x.To).NotEmpty().WithErrorCode(Common.Constants.INVALID_TO);

            RuleFor(x => x).Custom((x, context) =>
            {
                if (string.IsNullOrWhiteSpace(x.HtmlBody) && string.IsNullOrWhiteSpace(x.TextBody))
                {
                    context.AddFailure(GetValidationFailure(Common.Constants.INVALID_HTML_OR_TEXT_BODY));
                }
            });

            RuleFor(x => x.To).Custom((toAddresses, context) =>
            {
                var recepietnts = toAddresses.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                if (recepietnts.Any(r => !IsValidEmail(r)))
                {
                    context.AddFailure(GetValidationFailure(Common.Constants.INVALID_TO));
                }

                if (recepietnts.Count > 50)
                {
                    context.AddFailure(GetValidationFailure(Common.Constants.MAX_TO));
                }
            });
            RuleFor(x => x.Cc).Custom((ccAddresses, context) =>
            {
                if (!string.IsNullOrWhiteSpace(ccAddresses))
                {
                    var ccs = ccAddresses.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();

                    if (ccs.Any(r => !IsValidEmail(r)))
                    {
                        context.AddFailure(GetValidationFailure(Common.Constants.INVALID_CC));
                    }

                    if (ccs.Count > 50)
                    {
                        context.AddFailure(GetValidationFailure(Common.Constants.MAX_CC));
                    }
                }
            });
            RuleFor(x => x.Bcc).Custom((bccAddresses, context) =>
            {
                if (!string.IsNullOrWhiteSpace(bccAddresses))
                {
                    var bccs = bccAddresses.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();

                    if (bccs.Any(r => !IsValidEmail(r)))
                    {
                        context.AddFailure(GetValidationFailure(Common.Constants.INVALID_BCC));
                    }

                    if (bccs.Count > 50)
                    {
                        context.AddFailure(GetValidationFailure(Common.Constants.MAX_BCC));
                    }
                }
            });
        }

        private bool IsValidEmail(string email)
        {
            return _emailRegex.IsMatch(email);
        }
        private ValidationFailure GetValidationFailure(string errorCode)
        {
            var failure = new ValidationFailure("", "");
            failure.ErrorCode = errorCode;
            return failure;
        }
    }
}

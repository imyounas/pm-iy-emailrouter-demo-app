using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ER.Application.Validators.Email
{
    public class EmailRequestHeaderValidator : AbstractValidator<Dictionary<string, string>>
    {
        public EmailRequestHeaderValidator()
        {
            RuleFor(dic => dic).Custom((dic, context) =>
            {
                dic.TryGetValue(Common.Constants.ACCEPT_HEADER, out string accpHeader);
                dic.TryGetValue(Common.Constants.CONTENT_TYPE_HEADER, out string contpHeader);
                dic.TryGetValue(Common.Constants.PM_SERVER_TOKEN, out string pmToken);

                if (String.IsNullOrWhiteSpace(accpHeader) || String.IsNullOrWhiteSpace(contpHeader) || String.IsNullOrWhiteSpace(pmToken))
                {
                    context.AddFailure(new ValidationFailure("", "") { ErrorCode = Common.Constants.INVALID_HTTP_HEADERS });
                }

            });
        }
    }
}

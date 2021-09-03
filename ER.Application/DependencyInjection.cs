using ER.Application.Interfaces.PostProcessors;
using ER.Application.Models.Email;
using ER.Application.PostProcessors;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ER.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddSingleton(Assembly.GetExecutingAssembly());

            // post processors
            //services.AddTransient(typeof(IUnsubscribedUsersFilterProcessor<>), typeof(UnsubscribedUsersFilterProcessor));
            //services.AddTransient(typeof(IEmailBodyNLPProcessor<>), typeof(EmailBodyNLPProcessor));
            //services.AddTransient(typeof(IMessageReputationProcessor<>), typeof(MessageReputationProcessor));
            //services.AddTransient(typeof(ISenderReputationProcessor<>), typeof(SenderReputationProcessor));

            // TODO: Need to check here , having some type resolution issues
            services.AddTransient<IUnsubscribedUsersFilterProcessor<EmailMessageRequest>, UnsubscribedUsersFilterProcessor>();
            services.AddTransient<IEmailBodyNLPProcessor<EmailMessageRequest>, EmailBodyNLPProcessor>();
            services.AddTransient<IMessageReputationProcessor<EmailMessageRequest>, MessageReputationProcessor>();
            services.AddTransient<ISenderReputationProcessor<EmailMessageRequest>, SenderReputationProcessor>();

            return services;
        }
    }
}

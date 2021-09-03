using ER.Application.Interfaces.Cache;
using ER.Application.Interfaces.Database;
using ER.Application.Interfaces.MessageQueue;
using ER.Infrastructure.Cache;
using ER.Infrastructure.Database;
using ER.Infrastructure.MessageQueueHandler;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.ObjectPool;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ER.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services)
        {
            services.AddTransient<IApplicationDbContext, ApplicationDbContext>();
            services.AddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();
            services.AddSingleton<IPooledObjectPolicy<IModel>, RabbitMQPooledObjectPolicy>();
            services.AddSingleton<IMQPublisher, MQPublisher>();
            services.AddSingleton<ICacheManager, CacheManager>();


            return services;
        }
    }
}

using ER.Application.Interfaces.MessageQueue;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ER.Infrastructure.MessageQueueHandler
{
    public class MQPublisher : IMQPublisher
    {
        private readonly DefaultObjectPool<IModel> _mqConnectionPool;
        private readonly ILogger<RabbitMQPooledObjectPolicy> _logger;
        private readonly string _exchangeType;
        public MQPublisher(IPooledObjectPolicy<IModel> objectPolicy, ILogger<RabbitMQPooledObjectPolicy> logger)
        {
            _logger = logger;
            _mqConnectionPool = new DefaultObjectPool<IModel>(objectPolicy, Application.Common.Constants.MAX_RETAINED_MQ_CONNECTIONS);
            _exchangeType = ExchangeType.Topic;
        }

        public async Task<bool> PublishAsync<T>(string exchangeName, string queue, T message, string routeKey)
        {
            bool status = true;

            if (message == null)
            {
                status = false;
                return status;
            }

            var _channel = _mqConnectionPool.Get();

            try
            {

                _channel.ExchangeDeclare(exchange: exchangeName,
                                    type: _exchangeType,
                                    durable: true,
                                    autoDelete: false,
                                    arguments: null);

                _channel.QueueDeclare(queue: queue,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

                var messageBody = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

                var properties = _channel.CreateBasicProperties();
                properties.Persistent = true;
                properties.MessageId = Guid.NewGuid().ToString();
                properties.Type = message.GetType().FullName;

                _channel.BasicPublish(exchange: exchangeName,
                                    routingKey: queue,
                                    basicProperties: properties,
                                    body: messageBody);

            }

            catch (Exception ex)
            {
                _logger.LogError($"Some error while publishing message to [{queue}]. Error: [{ex.Message}]");
                status = false;
            }
            finally
            {
                _mqConnectionPool.Return(_channel);
            }

            return status;
        }
    }
}

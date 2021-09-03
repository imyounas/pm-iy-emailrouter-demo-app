using ER.Application.Common;
using ER.Application.Mediators.Command;
using ER.Application.Models.Email;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PM.IY.EmailRouterDemoApp.BackgroundServices
{
    /// <summary>
    /// should be separate hosted service to consume the messages and route them based on their type and meta data
    /// but for the sake of brevity adding into same project
    /// </summary>
    public class MQIngestEmailMessageService : BackgroundService
    {

        private readonly IModel _channel;
        private readonly DefaultObjectPool<IModel> _mqConnectionPool;
        private readonly AppSettings _appSettings;
        private readonly string _exchangeType;
        private readonly ISender _mediator;
        private readonly ILogger<MQIngestEmailMessageService> _logger;

        public MQIngestEmailMessageService(ISender mediator, IPooledObjectPolicy<IModel> objectPolicy, AppSettings appSettings, ILogger<MQIngestEmailMessageService> logger)
        {
            _appSettings = appSettings;
            _logger = logger;
            _mqConnectionPool = new DefaultObjectPool<IModel>(objectPolicy, ER.Application.Common.Constants.MAX_RETAINED_MQ_CONNECTIONS);
            _channel = _mqConnectionPool.Get();
            _exchangeType = ExchangeType.Topic;
            _mediator = mediator;

            Initialize(_appSettings.QueueExchange, _exchangeType, _appSettings.MessageInjestQueue, _appSettings.MessageInjestQueue);
        }

        public void Initialize(string exchangeName, string exchangeType, string queue, string routeKey)
        {
            try
            {
                _channel.ExchangeDeclare(exchange: exchangeName,
                                 type: exchangeType,
                                 durable: true,
                                 autoDelete: false,
                                 arguments: null);

                _channel.QueueDeclare(queue: queue,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

                _channel.QueueBind(queue, exchangeName, routeKey, null);
                _channel.BasicQos(0, 1, false);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Initializing in MQInjestEmailMessageService. Error [{ex.Message}]");
            }

        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var consumer = new EventingBasicConsumer(_channel);

            string content = "";
            consumer.Received += (ch, e) =>
            {
                content = Encoding.UTF8.GetString(e.Body.ToArray());
                var emailRequestMessage = JsonConvert.DeserializeObject<EmailMessageRequest>(content);

                var command = new PostProcessMQEmailMessageCommand()
                {
                    EmailRequestMessage = emailRequestMessage
                };

                var res = _mediator.Send(command);

                _channel.BasicAck(e.DeliveryTag, false);
            };

            consumer.Shutdown += Consumer_Shutdown; ;
            consumer.Registered += Consumer_Registered; ;
            consumer.Unregistered += Consumer_Unregistered; ;
            consumer.ConsumerCancelled += Consumer_ConsumerCancelled; ;

            _channel.BasicConsume(_appSettings.MessageInjestQueue, false, consumer);

            return Task.CompletedTask;
        }

        private void Consumer_ConsumerCancelled(object sender, ConsumerEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void Consumer_Unregistered(object sender, ConsumerEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void Consumer_Registered(object sender, ConsumerEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void Consumer_Shutdown(object sender, ShutdownEventArgs e)
        {
            //throw new NotImplementedException();
        }
    }
}

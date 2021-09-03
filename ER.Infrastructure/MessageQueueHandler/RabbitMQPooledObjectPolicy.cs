﻿using ER.Application.Common;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ER.Infrastructure.MessageQueueHandler
{
    // Source Refrence : https://www.c-sharpcorner.com/article/publishing-rabbitmq-message-in-asp-net-core/
    public class RabbitMQPooledObjectPolicy : IPooledObjectPolicy<IModel>
    {
        private readonly AppSettings _appSettings;
        private readonly ILogger<RabbitMQPooledObjectPolicy> _logger;
        private readonly IConnection _connection;

        public RabbitMQPooledObjectPolicy(AppSettings appSettings, ILogger<RabbitMQPooledObjectPolicy> logger)
        {
            _appSettings = appSettings;
            _logger = logger;
            _connection = GetConnection();

        }

        private IConnection GetConnection()
        {
            var factory = new ConnectionFactory()
            {
                HostName = _appSettings.QueueHost,
                UserName = _appSettings.QueueUserName,
                Password = _appSettings.QueuePassword,
                Port = _appSettings.QueuePort
            };

            try
            {
                _logger.LogInformation($"[RabbitMQPooledObjectPolicy] GetConnection [{_appSettings.QueueHost}] , [{_appSettings.QueueUserName}] , [{_appSettings.QueuePassword}] ,  [{_appSettings.QueuePort}]");
                return factory.CreateConnection();
            }
            catch (BrokerUnreachableException ex)
            {
                _logger.LogError($"[RabbitMQPooledObjectPolicy] Error while setting up RabbitMQ Connection. Error [{ex.Message}]");
                _logger.LogError($"[RabbitMQPooledObjectPolicy] Sleeping for 1 second and trying again");
                Thread.Sleep(1000);
                return factory.CreateConnection();
            }

          
        }

        public IModel Create()
        {
            return _connection.CreateModel();
        }

        public bool Return(IModel obj)
        {
            if (obj.IsOpen)
            {
                return true;
            }
            else
            {
                obj?.Dispose();
                return false;
            }
        }
    }
}

using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging
{
    public class Consumer : IDisposable
    {
        private readonly string _queueName;
        private readonly string _hostName;

        private readonly IConnection _connection;
        private readonly IModel _channel;

        public Consumer(string queueName, string hostName)
        {
            _queueName = queueName;
            _hostName = hostName;
            var factory = new ConnectionFactory()
            {
                HostName = _hostName,
                Port = 5671,
                UserName = "kjubdcol",
                Password = "zEFpskR90q2tfqgQSTKcYkAZ9qEi20_C",
                VirtualHost = "kjubdcol"
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public void Receive(EventHandler<BasicDeliverEventArgs> receiveCallback)
        {
            _channel.ExchangeDeclare("direct_exchange",
                "direct");

            _channel.QueueDeclare(_queueName,
                false,
                false,
                false,
                null);

            _channel.QueueBind(_queueName,
                "direct_exchange",
                _queueName);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += receiveCallback;

            _channel.BasicConsume(_queueName, true, consumer);
        }

        public void Dispose()
        {
            _connection?.Dispose();
            _channel?.Dispose();
        }
    }
}

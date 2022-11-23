using RabbitMQ.Client;
using System.Text;

namespace Messaging
{
    public class Producer
    {
        private readonly string _queueName;
        private readonly string _hostName;

        public Producer(string queueName, string hostName)
        {
            _queueName = queueName;
            _hostName = hostName;
        }

        public void Send(string message)
        {
            var factory = new ConnectionFactory()
            {
                HostName = _hostName,
                Port = 5672,
                UserName = "kjubdcol",
                Password = "zEFpskR90q2tfqgQSTKcYkAZ9qEi20_C",
                VirtualHost = "kjubdcol"
            };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.ExchangeDeclare("direct_exchange",
                "direct",
                false,
                false,
                null);

            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish("direct_exchange",
                _queueName,
                null,
                body);
        }
    }
}
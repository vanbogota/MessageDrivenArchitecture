using Messaging;
using Microsoft.Extensions.Hosting;
using System.Text;

namespace TableReservation.Notification
{
    public class Worker : BackgroundService
    {
        private readonly Consumer _consumer;
        public Worker()
        {
            _consumer = new Consumer("BookingNotification", "shrimp.rmq.cloudamqp.com");
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _consumer.Receive((sender, args) =>
            {
                var body = args.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(" [x] Recieved {0}", message);
            });
        }
    }
}

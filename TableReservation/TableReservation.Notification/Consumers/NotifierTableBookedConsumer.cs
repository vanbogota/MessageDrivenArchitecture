using MassTransit;
using Microsoft.Extensions.Logging;
using TableReservation.Messages;

namespace TableReservation.Notification.Consumers
{
    public class NotifierTableBookedConsumer : IConsumer<ITableBooked>
    {
        private readonly Notifier _notifier;
        private readonly ILogger<NotifierTableBookedConsumer> _logger;

        public NotifierTableBookedConsumer(Notifier notifier, ILogger<NotifierTableBookedConsumer> logger)
        {
            _notifier = notifier;
            _logger = logger;
        }

        public Task Consume(ConsumeContext<ITableBooked> context)
        {
            _logger.Log(LogLevel.Information, $"[OrderId: {context.Message.OrderId} CreationDate: {context.Message.CreationDate}]");

            var result = context.Message.Success;

            _notifier.Accept(context.Message.OrderId, 
                result ? Accepted.Booking : Accepted.Rejected);

            return Task.CompletedTask;
        }
    }
}

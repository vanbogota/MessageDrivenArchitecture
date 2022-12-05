using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using TableReservation.Messages;

namespace TableReservation.Notification.Consumers
{
    public class NotifyConsumer : IConsumer<INotify>
    {
        private readonly Notifier _notifier;
        private readonly ILogger<NotifyConsumer> _logger;

        public NotifyConsumer(Notifier notifier, ILogger<NotifyConsumer> logger)
        {
            _notifier = notifier;
            _logger = logger;
        }

        public Task Consume(ConsumeContext<INotify> context)
        {
            _logger.Log(LogLevel.Information, $"[OrderId: {context.Message.OrderId} ClientId: {context.Message.ClientId}]");

            _notifier.Notify(context.Message.OrderId, context.Message.ClientId, context.Message.Message);

            return context.ConsumeCompleted;
        }
    }
}
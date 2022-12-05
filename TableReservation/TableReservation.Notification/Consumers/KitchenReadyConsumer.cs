using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TableReservation.Messages;

namespace TableReservation.Notification.Consumers
{
    public class KitchenReadyConsumer : IConsumer<IKitchenReady>
    {

        private readonly Notifier _notifier;
        private readonly ILogger<KitchenReadyConsumer> _logger;

        public KitchenReadyConsumer(Notifier notifier, ILogger<KitchenReadyConsumer> logger)
        {
            _notifier = notifier;
            _logger = logger;
        }

        public Task Consume(ConsumeContext<IKitchenReady> context)
        {
            _logger.Log(LogLevel.Information, $"[OrderId: {context.Message.OrderId}]");

            _notifier.Accept(context.Message.OrderId, Accepted.Kitchen);

            return Task.CompletedTask;
        }
    }
}

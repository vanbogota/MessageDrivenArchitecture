using MassTransit;
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

        public KitchenReadyConsumer(Notifier notifier)
        {
            _notifier = notifier;
        }

        public Task Consume(ConsumeContext<IKitchenReady> context)
        {
            _notifier.Accept(context.Message.OrderId, Accepted.Kitchen);

            return Task.CompletedTask;
        }
    }
}

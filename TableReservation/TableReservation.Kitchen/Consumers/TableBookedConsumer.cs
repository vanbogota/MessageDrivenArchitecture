using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using TableReservation.Messages;

namespace TableReservation.Kitchen.Consumers
{
    public class KitchenBookingRequestedConsumer : IConsumer<IBookingRequest>
    {
        private readonly Manager _manager;
        private readonly ILogger<KitchenBookingRequestedConsumer> _logger;

        public KitchenBookingRequestedConsumer(Manager manager, ILogger<KitchenBookingRequestedConsumer> logger)
        {
            _manager = manager;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<IBookingRequest> context)
        {
            _logger.Log(LogLevel.Information, $"[OrderId: {context.Message.OrderId} CreationDate: {context.Message.CreationDate}]");

            _logger.Log(LogLevel.Debug, "Trying time: " + DateTime.Now);
            
            if (_manager.CheckKitchenReady(context.Message.OrderId, context.Message.PreOrder))
                await context.Publish<IKitchenReady>(new KitchenReady(context.Message.OrderId, true));
        }
    }
}
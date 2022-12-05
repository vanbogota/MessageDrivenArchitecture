using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using TableReservation.Messages;

namespace TableReservation.Kitchen.Consumers
{
    public class KitchenBookingRequestFaultConsumer : IConsumer<Fault<IBookingRequest>>
    {
        private readonly ILogger<KitchenBookingRequestFaultConsumer> _logger;

        public KitchenBookingRequestFaultConsumer(ILogger<KitchenBookingRequestFaultConsumer> logger)
        {
            _logger = logger;
        }
        public Task Consume(ConsumeContext<Fault<IBookingRequest>> context)
        {
            _logger.Log(LogLevel.Information, $"[OrderId {context.Message.Message.OrderId}] Отмена на кухне");
            return Task.CompletedTask;
        }
    }
}
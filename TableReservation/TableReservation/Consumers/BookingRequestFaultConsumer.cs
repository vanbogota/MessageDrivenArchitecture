using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using TableReservation.Messages;

namespace TableReservation.Consumers
{
    public class BookingRequestFaultConsumer : IConsumer<Fault<IBookingRequest>>
    {
        private readonly ILogger<BookingRequestFaultConsumer> _logger;

        public BookingRequestFaultConsumer(ILogger<BookingRequestFaultConsumer> logger)
        {
            _logger = logger;
        }
        public Task Consume(ConsumeContext<Fault<IBookingRequest>> context)
        {
            _logger.Log(LogLevel.Information, $"[OrderId {context.Message.Message.OrderId}] Отмена в зале");
            
            return Task.CompletedTask;
        }
    }
}
using System;
using System.Threading.Tasks;
using MassTransit;
using TableReservation.Messages;

namespace TableReservation.Kitchen.Consumers
{
    public class KitchenBookingRequestFaultConsumer : IConsumer<Fault<IBookingRequest>>
    {
        public Task Consume(ConsumeContext<Fault<IBookingRequest>> context)
        {
            Console.WriteLine($"[OrderId {context.Message.Message.OrderId}] Отмена на кухне");
            return Task.CompletedTask;
        }
    }
}
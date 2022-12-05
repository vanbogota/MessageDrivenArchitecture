using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using TableReservation.Messages;

namespace TableReservation.Kitchen.Consumers
{
    public class KitchenTableBookedConsumer : IConsumer<ITableBooked>
    {
        private readonly Manager _manager;
        private readonly ILogger<KitchenTableBookedConsumer> _logger;

        public KitchenTableBookedConsumer(Manager manager, ILogger<KitchenTableBookedConsumer> logger)
        {
            _manager = manager;
            _logger = logger;
        }

        public Task Consume(ConsumeContext<ITableBooked> context)
        {
            _logger.Log(LogLevel.Information, $"[OrderId: {context.Message.OrderId} CreationDate: {context.Message.CreationDate}]");
            
            var result = context.Message.Success;

           if (result)
               _manager.CheckKitchenReady(context.Message.OrderId, Dish.Pizza);
           
           return context.ConsumeCompleted;
        }
    }
}
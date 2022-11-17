using System.Threading.Tasks;
using MassTransit;
using TableReservation.Messages;

namespace TableReservation.Kitchen.Consumers
{
    public class KitchenTableBookedConsumer : IConsumer<ITableBooked>
    {
        private readonly Manager _manager;

        public KitchenTableBookedConsumer(Manager manager)
        {
            _manager = manager;
        }

        public Task Consume(ConsumeContext<ITableBooked> context)
        {
           var result = context.Message.Success;

           if (result)
               _manager.CheckKitchenReady(context.Message.OrderId, Dish.Pizza);
           
           return context.ConsumeCompleted;
        }
    }
}
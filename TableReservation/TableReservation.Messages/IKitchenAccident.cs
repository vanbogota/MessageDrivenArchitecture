using System;

namespace TableReservation.Messages
{
    public interface IKitchenAccident
    {
        public Guid OrderId { get; }
        
        public Dish Dish { get; }
    }
}
using System;
using MassTransit;
using TableReservation.Messages;

namespace TableReservation.Kitchen
{
    public class Manager
    {
        private readonly IBus _bus;

        public Manager(IBus bus)
        {
            _bus = bus;
        }

        public bool CheckKitchenReady(Guid orderId, Dish? dish)
        {
            return true;
        }
    }
}
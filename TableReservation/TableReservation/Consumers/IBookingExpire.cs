using System;

namespace TableReservation.Consumers
{
    public interface IBookingExpire
    {
        public Guid OrderId { get; }
    }
}
using System;

namespace TableReservation.Messages;

public interface IBookingCancellation
{
    public Guid OrderId { get; }
}

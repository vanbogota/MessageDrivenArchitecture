using MassTransit;

namespace TableReservation
{
    public class RestaurantBooking : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }

        public int CurrentState { get; set; }
        public Guid OrderId { get; set; }
        public Guid ClientId { get; set; }
        public int ReadyEventStatus { set; get; }
        public Guid? ExpirationId { get; set; } 
    }
}

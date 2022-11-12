namespace TableReservation.Messages
{
    public interface IKitchenReady
    {
        public Guid OrderId { get; }
        
        public bool Ready { get; }
    }
}
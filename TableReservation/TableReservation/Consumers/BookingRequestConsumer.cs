using MassTransit;
using Microsoft.Extensions.Logging;
using TableReservation.Messages;
using TableReservation.Messages.InMemoryDb;

namespace TableReservation.Consumers
{
    public class BookingRequestConsumer : IConsumer<IBookingRequest>
    {
        private readonly Restaurant _restaurant;
        private readonly IInMemoryRepository<IBookingRequest> _repository;
        private readonly ILogger _logger;

        public BookingRequestConsumer(Restaurant restaurant,
            IInMemoryRepository<IBookingRequest> repository,
            ILogger logger)
        {
            _restaurant = restaurant;
            _repository = repository;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<IBookingRequest> context)
        {
            _logger.Log(LogLevel.Information, $"[OrderId: {context.Message.OrderId}]");

            var model = _repository.Get().FirstOrDefault(i => i.OrderId == context.Message.OrderId);

            if (model is null)
            {
                _logger.Log(LogLevel.Debug, "First time message");
                _repository.AddOrUpdate(context.Message);
                var result = await _restaurant.BookFreeTableAsync(1);
                await context.Publish<ITableBooked>(new TableBooked(context.Message.OrderId, result ?? false));
                return;
            }

            _logger.Log(LogLevel.Debug, "Second time message");
        }
    }
}
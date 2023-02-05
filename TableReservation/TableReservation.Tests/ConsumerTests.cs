using MassTransit;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TableReservation.Consumers;
using TableReservation.Messages;
using TableReservation.Messages.InMemoryDb;

namespace TableReservation.Tests;

[TestFixture]
public class ConsumerTests
{
    private ServiceProvider _provider;
    private ITestHarness _harness;

    [OneTimeSetUp]
    public async Task Init()
    {
        _provider = new ServiceCollection()
            .AddMassTransitTestHarness(cfg =>
            {
                cfg.AddConsumer<BookingRequestConsumer>();
            })
            .AddLogging()
            .AddTransient<Restaurant>()
            .AddSingleton<IInMemoryRepository<IBookingRequest>, InMemoryRepository<IBookingRequest>>()
            .BuildServiceProvider(true);

        _harness = _provider.GetTestHarness();

        await _harness.Start();
    }

    [OneTimeTearDown]
    public async Task TearDown()
    {
        await _harness.OutputTimeline(TestContext.Out, options => options.Now().IncludeAddress());
        await _provider.DisposeAsync();
    }


    [Test]
    public async Task Any_booking_request_consumed()
    {
        var orderIdd = Guid.NewGuid();

        await _harness.Bus.Publish(
            (IBookingRequest)new BookingRequest(
                orderIdd,
                Guid.NewGuid(),
                null,
                DateTime.Now));

        Assert.That(await _harness.Consumed.Any<IBookingRequest>());
        await _harness.OutputTimeline(TestContext.Out, options => options.Now().IncludeAddress());
    }

    [Test]
    public async Task Booking_request_consumer_published_table_booked_message()
    {
        var consumer = _harness.GetConsumerHarness<BookingRequestConsumer>();

        var orderId = NewId.NextGuid();
        var bus = _harness.Bus;

        await bus.Publish((IBookingRequest)
            new BookingRequest(orderId,
                orderId,
                null,
                DateTime.Now));

        Assert.That(consumer.Consumed.Select<IBookingRequest>()
            .Any(x => x.Context.Message.OrderId == orderId), Is.True);
        
        Assert.That(_harness.Published.Select<ITableBooked>()
            .Any(x => x.Context.Message.OrderId == orderId), Is.True);
        await _harness.OutputTimeline(TestContext.Out, options => options.Now().IncludeAddress());
    }


    [Test]
    public async Task Booking_request_fault_consumer_published_fault_message()
    {
        var consumer = _provider.GetRequiredService<IConsumerTestHarness<BookingRequestFaultConsumer>>();

        var orderId = NewId.NextGuid();
        var bus = _provider.GetRequiredService<IBus>();

        await bus.Publish<IBookingRequest>(
            new BookingRequest(orderId,
                orderId,
                null,
                DateTime.Now));

        Assert.That(consumer.Consumed.Select<IBookingRequest>()
            .Any(x => x.Context.Message.OrderId == orderId), Is.True);
        
        Assert.That(_harness.Published.Select<ITableBooked>()
            .Any(x => x.Context.Message.OrderId == orderId), Is.True);
        await _harness.OutputTimeline(TestContext.Out, options => options.Now().IncludeAddress());
    }
}
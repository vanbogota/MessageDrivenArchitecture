using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Hosting;
using TableReservation.Messages;

namespace TableReservation
{
    public class Worker : BackgroundService
    {
        private readonly IBus _bus;
        private readonly Restaurant _restaurant;

        public Worker(IBus bus, Restaurant restaurant)
        {
            _bus = bus;
            _restaurant = restaurant;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(10000, stoppingToken);
                Console.WriteLine("Привет! Желаете забронировать столик?");

                var dateTime = DateTime.Now;
                await _bus.Publish(
                    (IBookingRequest)new BookingRequest(NewId.NextGuid(), NewId.NextGuid(), null, dateTime),
                    stoppingToken);
            }
        }
    }
}
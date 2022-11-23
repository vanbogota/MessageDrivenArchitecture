using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using System;
using TableReservation.Consumers;

namespace TableReservation
{
    public static class Program
    {
        //static readonly string _url = "amqps://kjubdcol:zEFpskR90q2tfqgQSTKcYkAZ9qEi20_C@shrimp.rmq.cloudamqp.com/kjubdcol";
        //static readonly Uri uri = new Uri(_url);
        public static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            
            CreateHostBuilder(args).Build().Run();
        }
        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddMassTransit(x =>
                    {
                        x.AddConsumer<RestaurantBookingRequestConsumer>()
                            .Endpoint(e =>
                            {
                                e.Temporary = true;
                            });

                        x.AddConsumer<BookingRequestFaultConsumer>()
                            .Endpoint(e =>
                            {
                                e.Temporary = true;
                            });

                        x.AddSagaStateMachine<RestaurantBookingSaga, RestaurantBooking>()
                            .Endpoint(e => e.Temporary = true)
                            .InMemoryRepository();

                        x.AddDelayedMessageScheduler();

                        x.UsingRabbitMq((context, cfg) =>
                        {
                            cfg.UseDelayedMessageScheduler();
                            cfg.UseInMemoryOutbox();
                            cfg.ConfigureEndpoints(context);
                        });
                    });
                                        
                    //services.AddMassTransitHostedService(true);

                    services.AddTransient<Restaurant>();
                    services.AddTransient<RestaurantBooking>();
                    services.AddTransient<RestaurantBookingSaga>();                    
                    services.AddHostedService<Worker>();
                });       
    }
}
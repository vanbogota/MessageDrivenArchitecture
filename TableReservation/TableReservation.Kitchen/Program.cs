using MassTransit;
using MassTransit.Audit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TableReservation.Kitchen.Consumers;
using TableReservation.Messages;

namespace TableReservation.Kitchen
{
    public class Program
    {
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
                        services.AddSingleton<IMessageAuditStore, AuditStore>();

                        var serviceProvider = services.BuildServiceProvider();
                        var auditStore = serviceProvider.GetService<IMessageAuditStore>();

                        x.AddConsumer<KitchenBookingRequestedConsumer>(
                            configurator =>
                            {
                                configurator.UseScheduledRedelivery(r =>
                                {
                                    r.Intervals(TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(20),
                                        TimeSpan.FromSeconds(30));
                                });
                                configurator.UseMessageRetry(
                                    r =>
                                    {
                                        r.Incremental(3, TimeSpan.FromSeconds(1),
                                            TimeSpan.FromSeconds(2));
                                    }
                                );
                            })
                            .Endpoint(e =>
                            {
                                e.Temporary = true;
                            }); ;

                        x.AddConsumer<KitchenBookingRequestFaultConsumer>()
                            .Endpoint(e =>
                            {
                                e.Temporary = true;
                            });
                        x.AddDelayedMessageScheduler();

                        x.UsingRabbitMq((context, cfg) =>
                        {
                            cfg.UseDelayedMessageScheduler();
                            cfg.UseInMemoryOutbox();
                            cfg.ConfigureEndpoints(context);
                            cfg.ConnectSendAuditObservers(auditStore);
                            cfg.ConnectConsumeAuditObserver(auditStore);
                        });
                        
                    });

                    services.Configure<MassTransitHostOptions>(options =>
                    {
                        options.WaitUntilStarted = true;
                        options.StartTimeout = TimeSpan.FromSeconds(30);
                        options.StopTimeout = TimeSpan.FromMinutes(1);
                    });

                    services.AddSingleton<Manager>();

                    services.AddSingleton<MassTransitHostedService>();                                        
                });
    }
}
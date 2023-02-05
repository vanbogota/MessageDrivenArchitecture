using MassTransit;
using MassTransit.Audit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TableReservation.Messages;
using TableReservation.Notification.Consumers;

namespace TableReservation.Notification
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            CreateHostBuilder(args).Build().Run(); 
        }
        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    //services.AddHostedService<Worker>();
                    services.AddMassTransit(x =>
                    {
                        services.AddSingleton<IMessageAuditStore, AuditStore>();

                        var serviceProvider = services.BuildServiceProvider();
                        var auditStore = serviceProvider.GetService<IMessageAuditStore>();

                        x.AddConsumer<NotifyConsumer>();

                        x.UsingRabbitMq((context, cfg) =>
                        {
                            cfg.UseMessageRetry(r =>
                            {
                                r.Exponential(5,
                                    TimeSpan.FromSeconds(1),
                                    TimeSpan.FromSeconds(100),
                                    TimeSpan.FromSeconds(5));
                                r.Ignore<StackOverflowException>();
                                r.Ignore<ArgumentNullException>(x => x.Message.Contains("Consumer"));
                            });

                            cfg.ConfigureEndpoints(context);
                            cfg.ConnectSendAuditObservers(auditStore);
                            cfg.ConnectConsumeAuditObserver(auditStore);
                        });
                    });
                    services.AddSingleton<Notifier>();

                    services.Configure<MassTransitHostOptions>(options =>
                    {
                        options.WaitUntilStarted = true;
                        options.StartTimeout = TimeSpan.FromSeconds(30);
                        options.StopTimeout = TimeSpan.FromMinutes(1);
                    });
                });
    }
}

using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TableReservation.Kitchen.Consumers;

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
                        x.AddConsumer<KitchenTableBookedConsumer>();

                        x.UsingRabbitMq((context, cfg) =>
                        {
                            cfg.ConfigureEndpoints(context);
                        });
                    });

                    services.AddSingleton<Manager>();
                    services.AddOptions<MassTransitHostOptions>()
                    .Configure(options =>
                    {
                        options.WaitUntilStarted = true;
                    });
                    //services.AddMassTransitHostedService(true);
                });
    }
}
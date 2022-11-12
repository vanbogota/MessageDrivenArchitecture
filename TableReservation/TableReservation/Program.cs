using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace TableReservation
{
    public static class Program
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
                        x.UsingRabbitMq((context, cfg) =>
                        {
                            cfg.ConfigureEndpoints(context);
                        });
                    });

                    services.AddOptions<MassTransitHostOptions>()
                    .Configure(options =>
                    {
                        options.WaitUntilStarted = true;
                    });
                    //services.AddMassTransitHostedService(true);

                    services.AddTransient<Restaurant>();

                    services.AddHostedService<Worker>();
                });

        //предыдущая версия
        //static async Task Main(string[] args)
        //{
        //    Console.OutputEncoding = Encoding.UTF8;
        //    Restaurant restaurant = new Restaurant();
            
        //    while (true)
        //    {
        //        await Task.Delay(10000);

        //        Notifications.Message(Notification.Welcome);

        //        if (int.TryParse(Console.ReadLine(), out int choice) && choice is not (1 or 2))
        //        {
        //            Console.WriteLine("Enter 1 or 2");
        //            continue;
        //        }

        //        Notifications.Message(Notification.ChooseRecieving);

        //        if (int.TryParse(Console.ReadLine(), out int choiceOfRecieving) && choiceOfRecieving is not (3 or 4))
        //        {
        //            Console.WriteLine("Enter 3 or 4");
        //            continue;
        //        }

        //        var stopWatch = new Stopwatch();
        //        stopWatch.Start();

        //        if(choice == 1)
        //        {
        //            if (choiceOfRecieving == 3)
        //            {
        //                restaurant.BookFreeTableAsync(3);
        //            }
        //            else
        //            {
        //                restaurant.BookFreeTable(3);
        //            }
        //        }
        //        else
        //        {
        //            if (choiceOfRecieving == 3)
        //            {
        //                restaurant.EscapeBookingTableAsync(1);
        //            }
        //            else
        //            {
        //                restaurant.EscapeBookingTable(2);
        //            }
        //        }

        //        Notifications.Message(Notification.ThankYou);

        //        stopWatch.Stop();
        //        var ts = stopWatch.Elapsed;
        //        Console.WriteLine($"{ts.Seconds:00}:{ts.Milliseconds:00}\n");

        //    }
        //}
    }
}
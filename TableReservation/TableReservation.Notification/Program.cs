using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace TableReservation.Notification
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            CreateHostBuilder(args).Build().Run(); 
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext,services) =>
            {
                services.AddHostedService<Worker>();
            });
    }
}
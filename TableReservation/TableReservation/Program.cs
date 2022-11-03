using System.Diagnostics;
using System.Text;

namespace TableReservation
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Restaurant restaurant = new Restaurant();
            
            while (true)
            {
                await Task.Delay(10000);

                Notifications.Message(Notification.Welcome);

                if (int.TryParse(Console.ReadLine(), out int choice) && choice is not (1 or 2))
                {
                    Console.WriteLine("Enter 1 or 2");
                    continue;
                }

                Notifications.Message(Notification.ChooseRecieving);

                if (int.TryParse(Console.ReadLine(), out int choiceOfRecieving) && choiceOfRecieving is not (3 or 4))
                {
                    Console.WriteLine("Enter 3 or 4");
                    continue;
                }

                var stopWatch = new Stopwatch();
                stopWatch.Start();

                if(choice == 1)
                {
                    if (choiceOfRecieving == 3)
                    {
                        restaurant.BookFreeTableAsync(3);
                    }
                    else
                    {
                        restaurant.BookFreeTable(3);
                    }
                }
                else
                {
                    if (choiceOfRecieving == 3)
                    {
                        restaurant.EscapeBookingTableAsync(1);
                    }
                    else
                    {
                        restaurant.EscapeBookingTable(2);
                    }
                }

                Notifications.Message(Notification.ThankYou);

                stopWatch.Stop();
                var ts = stopWatch.Elapsed;
                Console.WriteLine($"{ts.Seconds:00}:{ts.Milliseconds:00}\n");

            }
        }
    }
}
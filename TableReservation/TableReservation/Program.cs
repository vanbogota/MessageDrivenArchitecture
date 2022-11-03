using System.Diagnostics;

namespace TableReservation
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Restaurant restaurant = new Restaurant();
            while (true)
            {
                Console.WriteLine("Hello, do you wanna book table?\n" +
                    "1 - we send a message via sms (async)\n" +
                    "2 - wait on line for the answer (sync)");
                if (int.TryParse(Console.ReadLine(), out int choice) && choice is not (1 or 2))
                {
                    Console.WriteLine("Enter 1 or 2");
                    continue;
                }

                var stopWatch = new Stopwatch();
                stopWatch.Start();

                if (choice == 1)
                {
                    restaurant.BookFreeTableAsync(1);
                }
                else
                {
                    restaurant.BookFreeTable(1);
                }
                Console.WriteLine("Thank you for booking!");

                stopWatch.Stop();
                var ts = stopWatch.Elapsed;
                Console.WriteLine($"{ts.Seconds:00}:{ts.Milliseconds:00}\n");

                Console.WriteLine("Hello, do you wanna exit booking table?\n" +
                    "1 - we send a message via sms (async)\n" +
                    "2 - wait on line for the answer (sync)");

                if (int.TryParse(Console.ReadLine(), out int exitChoice) && exitChoice is not (1 or 2))
                {
                    Console.WriteLine("Enter 1 or 2");
                    continue;
                }

                if (exitChoice == 1)
                {
                    restaurant.EscapeBookingTableAsync(1);
                }
                else
                {
                    restaurant.EscapeBookingTable(1);
                }
                Console.WriteLine("Thank you!");
            }
        }
    }
}
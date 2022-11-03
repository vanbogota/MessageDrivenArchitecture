using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableReservation
{
    internal class Restaurant
    {
        private readonly List<Table> _tables = new List<Table>();

        public Restaurant()
        {
            for (int i = 1; i <= 10; i++)
            {
                _tables.Add(new Table(i));
            }
        }

        public void BookFreeTable(int countOfPersons)
        {
            Console.WriteLine("Wait for a second I will choose a table for you and approve reservation. Stay on line...");
            var table = _tables.FirstOrDefault(table => 
            table.SeatsCount > countOfPersons && table.State == State.Free);
            Thread.Sleep(5000);
            table?.SetState(State.Booked);
            Console.WriteLine(table is null 
                ? "Sorry, but there is no aviable tables right now" 
                : $"Ready, you table number is {table.Id}");
        }

        public void BookFreeTableAsync(int countOfPersons)
        {
            Console.WriteLine("Wait for a second I'll choose a table for you and approve reservation. We'll send you a notification");
            Task.Run(async () =>
            {
                var table = _tables.FirstOrDefault(table =>
                table.SeatsCount > countOfPersons && table.State == State.Free);
                await Task.Delay(1000 * 5);
                table?.SetState(State.Booked);
                Console.WriteLine(table is null
                ? "NOTIFICATION: Sorry, but there is no aviable tables right now"
                : $"NOTIFICATION: Ready, you table number is {table.Id}");
            });
        }

        public void EscapeBookingTable(int tableId)
        {
            Console.WriteLine("Wait for second I'll escape you booking. Stay on line...");
            var table = _tables.FirstOrDefault(table =>
            table.Id == tableId);
            Thread.Sleep(5000);
            if (table?.State == State.Free)
            {
                Console.WriteLine("Sorry, but it is already free");
            }
            Console.WriteLine(table is null
                ? "Sorry, but there is no such number of table"
                : $"Ready, table number {table.Id} is free");
        }

        public void EscapeBookingTableAsync(int tableId)
        {
            Console.WriteLine("We'll send you a message with result");
            Task.Run(async () =>
            {
                var table = _tables.FirstOrDefault(table =>
                table.Id == tableId);
                await Task.Delay(1000 * 5);
                table?.SetState(State.Free);
                Console.WriteLine(table is null
                ? "NOTIFICATION: Sorry, but there is no such number of table"
                : $"NOTIFICATION: Ready, table number {table.Id} is free");
            });
        }
    }
}

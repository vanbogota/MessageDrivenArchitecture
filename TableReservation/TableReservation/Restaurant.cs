using Messaging;
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

        //private readonly Producer _producer = new("BookingNotification", "shrimp.rmq.cloudamqp.com");

        public Restaurant()
        {
            for (int i = 1; i <= 10; i++)
            {
                _tables.Add(new Table(i));
            }
        }

        public void BookFreeTable(int countOfPersons)
        {
            Notifications.Message(Notification.WaitOnLine);

            var table = _tables.FirstOrDefault(table => 
            table.SeatsCount > countOfPersons && table.State == State.Free);
            Thread.Sleep(5000);
            table?.SetState(State.Booked);

            Notifications.BookingResultAsync(table);
        }

        public void BookFreeTableAsync(int countOfPersons)
        {
            Notifications.Message(Notification.WaitAsync);
            Task.Run(async () =>
            {
                var table = _tables.FirstOrDefault(table =>
                table.SeatsCount > countOfPersons && table.State == State.Free);
                await Task.Delay(1000 * 5);
                table?.SetState(State.Booked);

                Notifications.BookingResultAsync(table);
            });
        }

        public void EscapeBookingTable(int tableId)
        {
            Notifications.Message(Notification.EscapeOnLine);

            var table = _tables.FirstOrDefault(table =>
            table.Id == tableId && table.State == State.Booked);
            Thread.Sleep(5000);
            table?.SetState(State.Free);
            
            Notifications.EscapeResultAsync(table);
        }

        public void EscapeBookingTableAsync(int tableId)
        {
            Notifications.Message(Notification.EscapeAsync);

            Task.Run(async () =>
            {
                var table = _tables.FirstOrDefault(table =>
                table.Id == tableId && table.State == State.Booked);
                await Task.Delay(1000 * 5);
                table?.SetState(State.Free);

                Notifications.EscapeResultAsync(table);
            });
        }
    }
}

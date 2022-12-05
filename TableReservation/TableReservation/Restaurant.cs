using Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableReservation
{
    public class Restaurant
    {
        private readonly List<Table> _tables = new List<Table>();

        private readonly Producer _producer = new("BookingNotification", "localhost");

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
        #region for lesson 2
        //для урока 2
        //public void BookFreeTableAsync(int countOfPersons)
        //{
        //    Console.WriteLine("Подождите секунду я подберу столик и подтвержу вашу бронь," +
        //                      "Вам придет уведомление");
        //    Task.Run(async () =>
        //    {
        //        var table = _tables.FirstOrDefault(t => t.SeatsCount > countOfPersons
        //                                                && t.State == State.Free);
        //        await Task.Delay(1000 * 5); //у нас нерасторопные менеджеры, 5 секунд они находятся в поисках стола
        //        table?.SetState(State.Booked);

        //        _producer.Send(table is null
        //            ? $"УВЕДОМЛЕНИЕ: К сожалению, сейчас все столики заняты"
        //            : $"УВЕДОМЛЕНИЕ: Готово! Ваш столик номер {table.Id}");
        //    });
        //}
        #endregion
        public async Task<bool?> BookFreeTableAsync(int countOfPersons)
        {
            Notifications.Message(Notification.WaitAsync);
            var table = _tables.FirstOrDefault(table =>
                table.SeatsCount > countOfPersons
                && table.State == State.Free);

            await Task.Delay(1000 * 5);

            Notifications.BookingResultAsync(table);
            return table?.SetState(State.Booked);
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

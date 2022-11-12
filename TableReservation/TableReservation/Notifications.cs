using Messaging;


namespace TableReservation
{
    public static class Notifications
    {
        private static readonly int waitingSeconds = 2;
        private static readonly Producer _producer = new("BookingNotification", "shrimp.rmq.cloudamqp.com");
        public static void Message(Notification notification)
        {
            Task.Run(async () =>
            {
                switch (notification)
                {
                    case Notification.Welcome:
                        Console.WriteLine("Hello, do you wanna book table or escape booking?\n" +
                            "1 - booking table\n" +
                            "2 - escape booking\n");
                        break;
                    case Notification.ChooseRecieving:
                        await Task.Delay(1000 * waitingSeconds);
                        Console.WriteLine("3 - we send a result via sms (async)\n" +
                            "4 - wait on line for the answer (sync)");
                        break;
                    case Notification.WaitOnLine:
                        await Task.Delay(1000 * waitingSeconds);
                        Console.WriteLine("Wait for a second I will choose a table for you and approve reservation. Stay on line...");
                        break;
                    case Notification.WaitAsync:
                        await Task.Delay(1000 * waitingSeconds);
                        Console.WriteLine("Wait for a second I'll choose a table for you and approve reservation. We'll send you a notification");
                        break;
                    case Notification.EscapeOnLine:
                        await Task.Delay(1000 * waitingSeconds);
                        Console.WriteLine("Wait for second I'll escape you booking. Stay on line...");
                        break;
                    case Notification.EscapeAsync:
                        await Task.Delay(1000 * waitingSeconds);
                        Console.WriteLine("We'll send you a message with result");
                        break;
                    case Notification.ThankYou:
                        Console.WriteLine("Thank you for choosing our restaurant!");
                        break;
                    default:
                        break;
                }
            });            
        } 
        public static void BookingResultAsync(Table table)
        {
            Task.Run(async () =>
            {
                await Task.Delay(1000 * waitingSeconds);

                _producer.Send(table is null
                ? "Sorry, but there is no aviable tables right now"
                : $"Ready, you table number is {table.Id}");
            });            
        }

        public static void EscapeResultAsync(Table table)
        {
            Task.Run(async () =>
            {
                await Task.Delay(1000 * waitingSeconds);

                _producer.Send(table is null
                ? "Sorry, but there is no such number of table in reservation"
                : $"Ready, table number {table.Id} is free");
            });            
        }
    }
}

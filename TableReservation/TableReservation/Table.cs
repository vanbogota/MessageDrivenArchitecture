﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableReservation
{
    public class Table
    {
        public int Id { get; }
        public int SeatsCount { get; }
        public State State { get; private set; }

        public Table(int id)
        {
            Id = id;
            SeatsCount = _random.Next(2, 5);
            State = State.Free;
        }

        public bool SetState(State state)
        {
            lock (_lock)
            {
                if (State == state)
                {
                    return false;
                }
                State = state;

                Task.Run(async () =>
                {
                    if (State == State.Booked)
                    {
                        await Task.Delay(1000 * 20);
                        State = State.Free;
                        Console.WriteLine($"Table {Id} is free");
                    }
                });

                return true;
            }            
        }
        private readonly object _lock = new object();
        private static readonly Random _random = new();
    }
}

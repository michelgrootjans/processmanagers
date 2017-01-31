﻿using System;

namespace ConsoleApp
{
    internal class TtlChecker<T> : IHandle<T>
    {
        private readonly IHandle<T> _handler;

        public TtlChecker(IHandle<T> handler)
        {
            _handler = handler;
        }

        public void Handle(T message)
        {
            var ttlMessage = message as IHaveATimeToLive;
            if (message != null && ttlMessage.DueTime < DateTime.Now)
            {
                Console.WriteLine("DROPPING ORDER!");
            }
            else
            {
                _handler.Handle(message);
            }
        }
    }
    internal class TtlOrderChecker : IHandleOrder
    {
        private readonly IHandleOrder _handler;

        public TtlOrderChecker(IHandleOrder handler)
        {
            _handler = handler;
        }

        public void Handle(Order order)
        {
            if (order.DueTime < DateTime.Now)
            {
                Console.WriteLine("DROPPING ORDER!");
            }
            else
            {
                _handler.Handle(order);
            }
        }
    }
}
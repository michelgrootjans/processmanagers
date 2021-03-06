﻿using System;
using System.Collections.Generic;

namespace ProcessManagers
{
    internal class MidgetHouse : IHandle<OrderPlaced>, IHandle<Message>
    {
        private readonly TopicBasedPubSub _pubSub;
//        private readonly Dictionary<Guid, IProcessManager> _processManagers = new Dictionary<Guid, IProcessManager>();
        private Action<Guid> _subscribeTo;

        public MidgetHouse(TopicBasedPubSub pubSub)
        {
            _pubSub = pubSub;
        }

        public void Handle(OrderPlaced message)
        {
//            var processManager = CreateProcessManagerFor(message);
//            _processManagers[message.CorrelationId] = processManager;
            _subscribeTo(message.CorrelationId);
        }

        public void Handle(Message message)
        {
            var processManager = new PayLastProcessManager(new Dev0Publisher());
            foreach (var messages in _pubSub.MessagesFor(message.CorrelationId))
            {
                processManager.Handle(message);
            }
            processManager.SetPublisher(_pubSub);
            processManager.Handle(message);
        }

        public void SubscribeWith(Action<Guid> action)
        {
            _subscribeTo = action;
        }

        private IProcessManager CreateProcessManagerFor(OrderPlaced message)
        {
            //if (message.Order.TableNumber % 2 == 0)
            //{
            //    return new PayFirstProcessManager(_pubSub);
            //}
            return new PayLastProcessManager(_pubSub);
        }
    }

    internal class Dev0Publisher : IPublisher
    {
        public void Publish<T>(T message) where T : Message
        {
        }
    }
}
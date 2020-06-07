using FatalError.Micro.Core.Bus;
using MicroRabbit.Transfering.Domain.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MicroRabbit.Transfering.Domain.EventHandlers
{
    public class TestEventHnadler : IEventHandler<OrderCreatedEvent>
    {
        public Task Handle(OrderCreatedEvent @event)
        {
            return Task.CompletedTask;
        }
    }
}

using FatalError.Micro.Core.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroRabbit.Transfering.Domain.Events
{
    public class OrderCreatedEvent:Event
    {
        public Guid Id { get; set; }

        public OrderCreatedEvent(Guid id)
        {
            Id = id;
        }
    }
}

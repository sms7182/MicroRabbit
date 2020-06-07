using FatalError.Micro.Core.Bus;
using MicroRabbit.Banking.Domain.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MicroRabbit.Banking.Domain.EventHandlers
{
    public class TransferEventReplyHandler : IEventHandler<TransferCreatedEventResponse>
    {
        public Task Handle(TransferCreatedEventResponse @event)
        {
            return Task.CompletedTask;
        }
    }
}

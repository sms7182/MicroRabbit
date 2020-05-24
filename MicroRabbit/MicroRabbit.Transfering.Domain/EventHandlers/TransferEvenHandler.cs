using FatalError.Micro.Core.Bus;
using MicroRabbit.Transfering.Domain.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MicroRabbit.Transfering.Domain.EventHandlers
{
    public class TransferEvenHandler : IEventHandler<TransferCreatedEvent>
    {
        public TransferEvenHandler()
        {

        }
        public Task Handle(TransferCreatedEvent @event)
        {
            return Task.CompletedTask;
        }
    }
}

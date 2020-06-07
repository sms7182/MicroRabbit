using FatalError.Micro.Core.Bus;
using MicroRabbit.Transfering.Domain.Events;
using MicroRabbit.Transfering.Domain.Interfaces;
using MicroRabbit.Transfering.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MicroRabbit.Transfering.Domain.EventHandlers
{
    public class TransferEvenHandler : IEventHandler<TransferCreatedEvent>
    {
       readonly ITransferRepository transferRepository;
        IEventBus eventBus;
        public TransferEvenHandler(ITransferRepository repository,IEventBus e_ventBus)
        {
            transferRepository = repository;
            eventBus = e_ventBus;
        }
        public Task Handle(TransferCreatedEvent @event)
        {
            transferRepository.Add(new TransferLog()
            {
                AccountFrom = @event.From,
                AccountBalance = @event.Amount,
                AccountTo = @event.To
            });
            eventBus.Reply<TransferCreatedEventResponse>(new TransferCreatedEventResponse());
            return Task.CompletedTask;
        }
    }
    
   
}

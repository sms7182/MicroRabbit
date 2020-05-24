

using FatalError.Micro.Core.Bus;
using MicroRabbit.Banking.Domain.Models;
using MicroRabbit.Transfering.Application.Interfaces;
using MicroRabbit.Transfering.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroRabbit.Transfering.Application.Services
{
    public class TransferService : ITransferService
    {
        private readonly ITransferRepository transferRepository;
        private readonly IEventBus bus;
        public TransferService(ITransferRepository repository,IEventBus eventBus)
        {
            transferRepository = repository;
            bus = eventBus;
        }
        public IEnumerable<TransferLog> GetTransferLogs()
        {
            return transferRepository.GetTransferLogs();
        }

        
    }
}

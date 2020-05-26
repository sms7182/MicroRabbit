
using MicroRabbit.Transfering.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroRabbit.Transfering.Domain.Interfaces
{
    public interface ITransferRepository
    {
        IEnumerable<TransferLog> GetTransferLogs();
     

        void Add(TransferLog transferLog);
    }
}

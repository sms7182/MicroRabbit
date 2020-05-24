
using MicroRabbit.Banking.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroRabbit.Transfering.Application.Interfaces
{
   public interface ITransferService
    {
        IEnumerable<TransferLog> GetTransferLogs();
       
    }
}

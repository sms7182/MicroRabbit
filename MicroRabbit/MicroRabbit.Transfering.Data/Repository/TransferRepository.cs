using MicroRabbit.Banking.Domain.Models;
using MicroRabbit.Transfering.Data.Context;
using MicroRabbit.Transfering.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroRabbit.Transfering.Data.Repository
{
    public class TransferRepository : ITransferRepository
    {
        private TransferingDBContext ctx;
        public TransferRepository(TransferingDBContext context)
        {
            ctx = context;
        }
        public IEnumerable<TransferLog> GetTransferLogs()
        {
          return  ctx.TransferLogs;
        }
    }
}

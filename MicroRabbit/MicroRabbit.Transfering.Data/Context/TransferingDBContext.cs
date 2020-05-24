using MicroRabbit.Banking.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroRabbit.Transfering.Data.Context
{
   public class TransferingDBContext:DbContext
    {
        public TransferingDBContext(DbContextOptions options):base(options)
        {

        }

        public DbSet<TransferLog> TransferLogs { get; set; }
    }
}

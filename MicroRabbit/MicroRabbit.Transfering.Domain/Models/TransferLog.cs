using System;
using System.Collections.Generic;
using System.Text;

namespace MicroRabbit.Banking.Domain.Models
{
    public class TransferLog
    {
        public Guid Id { get; set; }
        public int AccountFrom { get; set; }
        public int AccountTo { get; set; }

        public decimal AccountBalance { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MicroRabbit.Transfering.Domain.Models
{
    public class TransferLog
    {
        public TransferLog()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }
        public int AccountFrom { get; set; }
        public int AccountTo { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal AccountBalance { get; set; }

    }
}

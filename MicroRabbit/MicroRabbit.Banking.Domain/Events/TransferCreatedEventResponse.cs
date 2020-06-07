using FatalError.Micro.Core.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroRabbit.Banking.Domain.Events
{
    public class TransferCreatedEventResponse:Event
    {
        public TransferCreatedEventResponse()
        {
            Id = Guid.NewGuid();

        }
        public Guid Id { get; set; }
    }
}

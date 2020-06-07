using FatalError.Micro.Core.Commands;
using FatalError.Micro.Core.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FatalError.Micro.Core.Bus
{
    public interface IEventBus
    {
        Task SendCommand<T>(T command) where T : Command;
        void Publish<T>(T @event) where T : Event;

        void Subscribe<T, TH>()
            where T : Event
            where TH : IEventHandler<T>;

        void Subscribe<R, RH, T>()
            where R :Event 
            where RH : IEventHandler<R>
            where T : Event;
        void Reply<R>(R replyMessage) where R : Event;

    }
}

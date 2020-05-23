using FatalError.Micro.Core.Bus;
using FatalError.Micro.Core.Commands;
using FatalError.Micro.Core.Events;
using MediatR;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroRabbit.Infra.Bus
{
    public sealed class RabbitMQBus : IEventBus
    {
        private readonly IMediator mediator;
        private readonly Dictionary<string, List<Type>> handlres;
        private readonly List<Type> eventTypes;
        
        public RabbitMQBus(IMediator _mediator)
        {
            mediator = _mediator;
            handlres = new Dictionary<string, List<Type>>();
            eventTypes = new List<Type>();
        }
        public void Publish<T>(T @event) where T : Event
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost"
            };
            using(var connection = factory.CreateConnection())
            {
                using(var channel = connection.CreateModel())
                {
                    var eventname = @event.GetType().Name;
                    channel.QueueDeclare(eventname, false, false, false, null);
                    var message = JsonConvert.SerializeObject(@event);
                    var body = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish("", eventname, null, body);
                }
            }
        }

        public Task SendCommand<T>(T command) where T : Command
        {
            return mediator.Send(command);
        }

        public void Subscribe<T, TH>()
            where T : Event
            where TH : IEventHandler<T>
        {
            var eventName = typeof(T).Name;
            var handlerType = typeof(TH);
            if (!eventTypes.Contains(typeof(T)))
            {
                eventTypes.Add(typeof(T));
            }

            if (!handlres.ContainsKey(eventName))
            {
                handlres.Add(eventName, new List<Type>());
            }

            if (handlres[eventName].Any(s => s.GetType() == handlerType)){
                throw new ArgumentException($"Handler Type {handlerType.Name} already is registred for ${eventName}");
            }
            handlres[eventName].Add(handlerType);
            StartBasicConsume<T>();
        }
        private void StartBasicConsume<T>() where T : Event
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                DispatchConsumersAsync=true
            };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            var eventName = typeof(T).Name;
            channel.QueueDeclare(eventName, false, false, false,null);
            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.Received += Consumer_Received;
            channel.BasicConsume(eventName, true, consumer);
        }

        private async Task Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            var eventName = e.RoutingKey;
            var message = Encoding.UTF8.GetString(e.Body.ToArray());

            try{
                await ProcessEvent(eventName, message).ConfigureAwait(false);
            }
            catch(Exception ex)
            {
                
            }
        }
        private async Task ProcessEvent(string eventName,string message)
        {
            if(handlres.ContainsKey(eventName))
            {
                var subscriptions = handlres[eventName];
                foreach(var subscription in subscriptions)
                {
                    var handler = Activator.CreateInstance(subscription);
                    if(handler==null)
                    {
                        continue;
                    }
                    var eventType = eventTypes.SingleOrDefault(t => t.Name == eventName);
                    var @event = JsonConvert.DeserializeObject(message, eventType);
                    var convertType = typeof(IEventHandler<>).MakeGenericType(eventType);
                    await (Task)convertType.GetMethod("Handle").Invoke(handler, new object[] { @event });

                }
            }
        }
    }
}

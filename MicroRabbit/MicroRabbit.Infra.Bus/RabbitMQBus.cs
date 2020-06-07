using FatalError.Micro.Core.Bus;
using FatalError.Micro.Core.Commands;
using FatalError.Micro.Core.Events;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
        private readonly Dictionary<string, Type> handleMessages;
        private readonly List<Type> eventTypes;
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly IConfiguration configuration;
        private readonly Dictionary<string, Dictionary<string, Type>> repliesOfT;
        public RabbitMQBus(IMediator _mediator,IServiceScopeFactory serviceScope,IConfiguration _configuration)
        {
            mediator = _mediator;
            handlres = new Dictionary<string, List<Type>>();
            handleMessages = new Dictionary<string, Type>();
            repliesOfT = new Dictionary<string, Dictionary<string, Type>>();
            eventTypes = new List<Type>();
            serviceScopeFactory = serviceScope;
            configuration = _configuration;
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
        public void Reply<R>(R replyMessage)  where R : Event
        {

            Publish(replyMessage);
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

            if (handlres[eventName].Any(s => s == handlerType)){
                throw new ArgumentException($"Handler Type {handlerType.Name} already is registred for ${eventName}");
            }
            handlres[eventName].Add(handlerType);
            StartBasicConsume<T>();
        }
        public void Subscribe<R,RH,T>()
            where R:Event
            where RH:IEventHandler<R>
            where T:Event
           
        {
            var parentEventName = typeof(T).Name;
            var eventName = typeof(R).Name;
            var messageHandleType = typeof(RH);
            eventTypes.Add(typeof(R));
            if (repliesOfT.ContainsKey(eventName))
            {
                if (!repliesOfT[eventName].ContainsKey(parentEventName))
                {
                    repliesOfT[eventName].Add(parentEventName, messageHandleType);
                }
                else
                {
                    if (repliesOfT[eventName].ContainsKey(parentEventName))
                    {
                        throw new Exception($"ReplyHandle:{messageHandleType} is exist for Type T:{eventName}");
                    }
                    else
                    {
                        repliesOfT[eventName].Add(parentEventName, messageHandleType);
                    }
                }
            }
            else
            {
               var handlerType= new Dictionary<string, Type>();
                handlerType.Add(parentEventName, messageHandleType);
                repliesOfT.Add(eventName, handlerType);
            }
           
            StartBasicConsume<T, R>();
        }
           
        private void StartBasicConsume<T,R>() where T:Event where R : Event
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                DispatchConsumersAsync = true
            };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            var eventName = typeof(R).Name;
            channel.QueueDeclare(eventName, false, false, false, null);
            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.Received += Consumer_Received_Replied;

            channel.BasicConsume(eventName, true, consumer);
        }

        private async Task Consumer_Received_Replied(object sender, BasicDeliverEventArgs e)
        {
            var eventName = e.RoutingKey;
            var message = Encoding.UTF8.GetString(e.Body.ToArray());

            try
            {
                await ProcessEventReply(eventName, message).ConfigureAwait(false);
            }
            catch (Exception ex)
            {

            }
        }
        private async Task ProcessEventReply(string eventName, string message)
        {
         
            if (repliesOfT.ContainsKey(eventName))
            {
                
                using (var scope = serviceScopeFactory.CreateScope())
                {
                    
                    var eventReplies = repliesOfT[eventName];

                    for (int i = 0; i < eventReplies.Keys.Count; i++)
                    {
                        var parentEventName = eventReplies.Keys.ToList()[i];
                        var subscription = eventReplies[parentEventName];


                        var handler = scope.ServiceProvider.GetService(subscription);
                        if (handler == null)
                        {
                            throw new Exception();
                        }
                        var eventType = eventTypes.SingleOrDefault(t => t.Name == eventName);
                        var @event = JsonConvert.DeserializeObject(message, eventType);
                        var convertType = typeof(IEventHandler<>).MakeGenericType(eventType);
                        await (Task)convertType.GetMethod("Handle").Invoke(handler, new object[] { @event });
                    }
                    
                }

            }
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
                using(var scope = serviceScopeFactory.CreateScope())
                {
                    var subscriptions = handlres[eventName];
                    foreach (var subscription in subscriptions)
                    {
                        var handler = scope.ServiceProvider.GetService(subscription);
                        if (handler == null)
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
}

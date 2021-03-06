﻿using FatalError.Micro.Core.Bus;
using MediatR;
using MicroRabbit.Banking.Application.Interfaces;
using MicroRabbit.Banking.Application.Services;
using MicroRabbit.Banking.Data.Context;
using MicroRabbit.Banking.Data.Repository;
using MicroRabbit.Banking.Domain.CommandHandlers;
using MicroRabbit.Banking.Domain.Commands;
using MicroRabbit.Banking.Domain.EventHandlers;
using MicroRabbit.Banking.Domain.Interfaces;
using MicroRabbit.Infra.Bus;
using MicroRabbit.Transfering.Application.Interfaces;
using MicroRabbit.Transfering.Application.Services;
using MicroRabbit.Transfering.Data.Context;
using MicroRabbit.Transfering.Data.Repository;
using MicroRabbit.Transfering.Domain.EventHandlers;
using MicroRabbit.Transfering.Domain.Events;
using MicroRabbit.Transfering.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace FatalError.Micro.Infra.IoC
{
    public class DependencyContainer
    { 
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddSingleton<IEventBus, RabbitMQBus>(sp=>
            {
                var scopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
                var configuration=sp.GetRequiredService<IConfiguration>();
                return new RabbitMQBus(sp.GetService<IMediator>(), scopeFactory,configuration);
            });

            services.AddTransient<TransferEvenHandler>();
            services.AddTransient<TransferEventReplyHandler>();

            services.AddTransient<IEventHandler<TransferCreatedEvent>, TransferEvenHandler>();
            services.AddTransient<IEventHandler<MicroRabbit.Banking.Domain.Events.TransferCreatedEventResponse>, TransferEventReplyHandler>();
            services.AddTransient<IRequestHandler<CreateTransferCommand,bool>, TransferCommandHandler>();

          
            
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<ITransferService, TransferService>();


            services.AddTransient<IAccountRepository, AccountRepository>();
            services.AddTransient<ITransferRepository, TransferRepository>();
            services.AddTransient<BankingDBContext>();
            services.AddTransient<TransferingDBContext>();

        }
    }
}

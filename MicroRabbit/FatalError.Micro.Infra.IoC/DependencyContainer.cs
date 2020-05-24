using FatalError.Micro.Core.Bus;
using MediatR;
using MicroRabbit.Banking.Application.Interfaces;
using MicroRabbit.Banking.Application.Services;
using MicroRabbit.Banking.Data.Context;
using MicroRabbit.Banking.Data.Repository;
using MicroRabbit.Banking.Domain.CommandHandlers;
using MicroRabbit.Banking.Domain.Commands;
using MicroRabbit.Banking.Domain.Interfaces;
using MicroRabbit.Infra.Bus;
using MicroRabbit.Transfering.Application.Interfaces;
using MicroRabbit.Transfering.Application.Services;
using MicroRabbit.Transfering.Data.Context;
using MicroRabbit.Transfering.Data.Repository;
using MicroRabbit.Transfering.Domain.EventHandlers;
using MicroRabbit.Transfering.Domain.Events;
using MicroRabbit.Transfering.Domain.Interfaces;
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
            services.AddTransient<IEventBus, RabbitMQBus>();

            services.AddTransient<IEventHandler<TransferCreatedEvent>, TransferEvenHandler>();

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

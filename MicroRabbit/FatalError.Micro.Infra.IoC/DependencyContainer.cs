﻿using FatalError.Micro.Core.Bus;
using MediatR;
using MicroRabbit.Banking.Application.Interfaces;
using MicroRabbit.Banking.Application.Services;
using MicroRabbit.Banking.Data.Context;
using MicroRabbit.Banking.Data.Repository;
using MicroRabbit.Banking.Domain.CommandHandlers;
using MicroRabbit.Banking.Domain.Commands;
using MicroRabbit.Banking.Domain.Interfaces;
using MicroRabbit.Infra.Bus;
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


            services.AddTransient<IRequestHandler<CreateTransferCommand,bool>, TransferCommandHandler>();

            services.AddTransient<IAccountService, AccountService>();
            
            
            services.AddTransient<IAccountRepository, AccountRepository>();
            
            
            services.AddTransient<BankingDBContext>();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MicroRabbit.Banking.Application.Interfaces;
using MicroRabbit.Banking.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MicroRabbit.Banking.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BankingController : ControllerBase
    {
       
        private readonly ILogger<BankingController> _logger;
        private readonly IAccountService accountService;

        public BankingController(ILogger<BankingController> logger,IAccountService service)
        {
            _logger = logger;
            accountService = service;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Account>> Get()
        {
            return Ok(accountService.GetAccounts());
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MicroRabbit.Banking.Domain.Models;
using MicroRabbit.Transfering.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MicroRabbit.Transfering.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransferController : ControllerBase
    {
      

        private readonly ILogger<TransferController> _logger;

        private readonly ITransferService transferservice;
        public TransferController(ILogger<TransferController> logger,ITransferService service)
        {
            _logger = logger;
            transferservice = service;
        }

        [HttpGet]
        public ActionResult<IEnumerable<TransferLog>> GetTransferLogs()
        {
            return Ok(transferservice.GetTransferLogs());
        }
    }
}

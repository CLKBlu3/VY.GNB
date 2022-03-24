using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using VY.GNB.Business.Contracts.Services;
using VY.GNB.Dtos;
using VY.GNB.XCutting.Domain.OperationResult;

namespace VY.GNB.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly ITransactionsService _transactionsService;

        public TransactionsController(ITransactionsService transactionsService)
        {
            _transactionsService = transactionsService;
        }

        [HttpGet("All")]
        [ProducesResponseType(typeof(IEnumerable<TransactionDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<ErrorObject>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll()
        {
            var res = await _transactionsService.GetAll();
            if (res.HasErrors())
            {
                return BadRequest(res.Errors);
            }
            return Ok(res.Result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(IEnumerable<TransactionDtoContext>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<ErrorObject>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetByProductIdentifier([FromRoute] string id)
        {
            var res = await _transactionsService.GetById(id);
            if (res.HasErrors())
            {
                return BadRequest(res.Errors);
            }
            return Ok(res.Result);
        }
    }
}

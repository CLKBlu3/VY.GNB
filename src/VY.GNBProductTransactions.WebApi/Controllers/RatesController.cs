using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using VY.GNB.Business.Contracts.Services;
using VY.GNB.Dtos;
using VY.GNB.XCutting.Domain.OperationResult;

namespace VY.GNB.Controllers
{
    public class RatesController : Controller
    {
        private readonly IRatesService _ratesService;

        public RatesController(IRatesService ratesService)
        {
            _ratesService = ratesService;
        }

        [HttpGet("Get")]
        [ProducesResponseType(typeof(IEnumerable<RateDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<ErrorObject>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get()
        {
            var result = await _ratesService.GetAsync();
            if (result.HasErrors())
            {
                return BadRequest(result.Errors);
            }
            return Ok(result.Result);
        }
    }
}

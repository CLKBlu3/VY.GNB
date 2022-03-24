using System.Collections.Generic;
using System.Threading.Tasks;
using VY.GNB.Dtos;
using VY.GNB.XCutting.Domain.OperationResult;

namespace VY.GNB.Business.Contracts.Services
{
    public interface IRatesService
    {
        public Task<OperationResult<IEnumerable<RateDto>>> GetAsync();
    }
}

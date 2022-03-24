using System.Collections.Generic;
using System.Threading.Tasks;
using VY.GNB.Dtos;
using VY.GNB.XCutting.Domain.OperationResult;

namespace VY.GNB.Business.Contracts.Services
{
    public interface ITransactionsService
    {
        public Task<OperationResult<IEnumerable<TransactionDto>>> GetAll();
        public Task<OperationResult<TransactionDtoContext>> GetById(string id);
    }
}

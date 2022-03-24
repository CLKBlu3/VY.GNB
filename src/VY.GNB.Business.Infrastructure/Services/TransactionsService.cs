using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VY.GNB.Business.Contracts.Services;
using VY.GNB.Dtos;
using VY.GNB.Infrastructure.Contracts.Entities;
using VY.GNB.Infrastructure.Contracts.Repositories;
using VY.GNB.XCutting.Contracts.Services;
using VY.GNB.XCutting.Domain.OperationResult;

namespace VY.GNB.Business.Implementation.Services
{
    public class TransactionsService : ITransactionsService
    {
        private readonly ILogger<TransactionsService> _logger;
        private readonly IProxy<TransactionDto> _proxy;
        private readonly IRepository<Transaction> _repository;
        private readonly IMapper _mapper;
        private readonly IEurConverter _eurConverter;

        public TransactionsService(ILogger<TransactionsService> logger,
                                   IProxy<TransactionDto> proxy,
                                   IRepository<Transaction> repository,
                                   IMapper mapper,
                                   IEurConverter eurConverter)
        {
            _logger = logger;
            _proxy = proxy;
            _repository = repository;
            _mapper = mapper;
            _eurConverter = eurConverter;
        }

        #region .:ITransactionService Methods:.
        public async Task<OperationResult<IEnumerable<TransactionDto>>> GetAll()
        {
            OperationResult<IEnumerable<TransactionDto>> result = new OperationResult<IEnumerable<TransactionDto>>();
            var res = await _proxy.GetAsync();
            if (res.Any())
            {
                _logger.LogInformation("Retrieved transactions from proxy. Making a backup on the specified repository.");
                result.SetResult(res);
                try
                {
                    await Persist(res);
                    _logger.LogInformation("Data persisted in the specified repository.");
                }
                catch (Exception ex)
                {
                    _logger.LogError("Data was not persisted in repository due an exception.");
                    result.AddException(ex);
                }
                return result;
            }
            var resFromRepo = await _repository.GetAllAsync();
            if (!resFromRepo.Any())
            {
                _logger.LogInformation("Data for transactions was not found neither in Proxy nor repository.");
                result.AddError(2, "Couldn't find the transactions data neither in Proxy nor Repository.");
            }
            else
            {
                _logger.LogInformation("Retrieved transactions from Repository.");
                var resAsDto = _mapper.Map<IEnumerable<TransactionDto>>(resFromRepo);
                result.SetResult(resAsDto);
            }
            return result;
        }

        public async Task<OperationResult<TransactionDtoContext>> GetById(string id)
        {
            OperationResult<TransactionDtoContext> result = new OperationResult<TransactionDtoContext>();
            var transactions = _repository.GetById(id);
            if (transactions.Any())
            {
                var dto = _mapper.Map<IEnumerable<TransactionDto>>(transactions);
                var resConv = await _eurConverter.ConvertToEurAsync(dto);
                if (resConv.HasErrors())
                {
                    result.AddErrors(resConv.Errors);
                    return result;
                }
                result.SetResult(ConvertToContext(resConv.Result));
            }
            return result;
        }
        #endregion

        #region .:Private Methods:.
        private async Task Persist(IEnumerable<TransactionDto> res)
        {
            var entities = _mapper.Map<IEnumerable<Transaction>>(res);
            foreach (var entity in entities)
            {
                //adding a new Id before insertions
                entity.Id = Guid.NewGuid();
            }
            await _repository.AddAsync(entities);
            await _repository.SaveChangesAsync();
        }

        private TransactionDtoContext ConvertToContext(IEnumerable<TransactionDto> result)
        {
            var amount = 0.0;
            foreach(var entity in result)
            {
                amount += Double.Parse(entity.Amount, System.Globalization.CultureInfo.InvariantCulture);
            }
            TransactionDtoContext context = new TransactionDtoContext()
            {
                TransactionData = result,
                TotalAmount = amount
            };
            return context;
        }
        #endregion
    }
}

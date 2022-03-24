using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VY.GNB.Business.Contracts.Services;
using VY.GNB.Business.Implementation.Domain;
using VY.GNB.Dtos;
using VY.GNB.XCutting.Domain.OperationResult;

namespace VY.GNB.Business.Implementation.Services
{
    public class EurConverterService : IEurConverter
    {
        private readonly IRatesService _ratesService;
        private readonly IMapper _mapper;

        public EurConverterService(IRatesService ratesService,
                                   IMapper mapper)
        {
            _ratesService = ratesService;
            _mapper = mapper;
        }

        public async Task<OperationResult<IEnumerable<TransactionDto>>> ConvertToEurAsync(IEnumerable<TransactionDto> toConvert)
        {
            OperationResult<IEnumerable<TransactionDto>> result = new OperationResult<IEnumerable<TransactionDto>>();
            var rates = (await _ratesService.GetAsync());
            if (rates.HasErrors())
            {
                result.AddErrors(rates.Errors);
                return result;
            }
            foreach(var transaction in toConvert)
            {
                List<RateDomain> rateList = _mapper.Map<List<RateDomain>>(rates.Result);
                try
                {
                    if (!transaction.Currency.Trim().Equals("EUR")) {
                        ConvertCurrency(transaction, "EUR", rateList);
                    }
                }
                catch (Exception ex)
                {
                    result.AddException(ex);
                }
            }
            result.SetResult(toConvert);
            return result;
        }

        private void ConvertCurrency(TransactionDto transactionDto, string to, List<RateDomain> rates)
        {
            if(!rates.Any(c => c.IsVisited == false))
            {
                throw new Exception("Not Supported Currency conversion");
            }
            var baseCase = rates.FirstOrDefault(c => c.From.Trim().Equals(transactionDto.Currency.Trim()) && c.To.Trim().Equals(to.Trim())
                                                || c.From.Trim().Equals(to) && c.To.Trim().Equals(transactionDto.Currency));
            if(baseCase != default)
            {
                //Found direct conversion, case 1: direct
                if(transactionDto.Currency.Trim().Equals(baseCase.From.Trim()))
                {
                    transactionDto.Currency = to.Trim();
                    double aux = double.Parse(transactionDto.Amount, System.Globalization.CultureInfo.InvariantCulture)
                                            * baseCase.Ratio;
                    aux = Math.Round(aux, 2);
                    transactionDto.Amount = aux.ToString()
                                            .Trim();
                }
                else //Case 2: inverted
                {
                    transactionDto.Currency = baseCase.To.Trim();
                    double aux = double.Parse(transactionDto.Amount, System.Globalization.CultureInfo.InvariantCulture)
                                            * 1.0 / baseCase.Ratio;
                    aux = Math.Round(aux, 2);
                    transactionDto.Amount = aux.ToString()
                                            .Trim();
                }
            }
            else
            {
                var x = rates.FirstOrDefault(c => c.To.Trim().Equals(to.Trim()) && c.IsVisited == false);
                if(x != default)
                {
                    x.IsVisited = true;
                    //Convert recursively to x.To so that we can convert to the desired currency later.
                    ConvertCurrency(transactionDto, x.From, rates);
                    ConvertCurrency(transactionDto, to, rates);
                }
            }
        }
    }
}

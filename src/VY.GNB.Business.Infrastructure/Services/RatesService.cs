using ATC.RedisClient.Contracts.ServiceLibrary;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VY.GNB.Business.Contracts.Services;
using VY.GNB.Dtos;
using VY.GNB.XCutting.Contracts.Services;
using VY.GNB.XCutting.Domain.OperationResult;

namespace VY.GNB.Business.Implementation.Services
{
    public class RatesService : IRatesService
    {
        private readonly IProxy<RateDto> _proxy;
        private readonly IRedisCacheClient _redisCacheClient;
        private readonly ILogger<RatesService> _logger;

        public RatesService(IProxy<RateDto> proxy,
                            IRedisCacheClient redisCacheClient,
                            ILogger<RatesService> logger)
        {
            _proxy = proxy;
            _redisCacheClient = redisCacheClient;
            _logger = logger;
        }

        public async Task<OperationResult<IEnumerable<RateDto>>> GetAsync()
        {
            OperationResult<IEnumerable<RateDto>> result = new OperationResult<IEnumerable<RateDto>>();
            var rates = _redisCacheClient.Get<IEnumerable<RateDto>>("RateList");
            if(rates.IsSuccessfulOperation && rates.CacheValue.Any())
            {
                _logger.LogInformation("Retrieved rates from Redis.");
                result.SetResult(rates.CacheValue);
                return result;
            }
            var ratesFromProxy = await _proxy.GetAsync();
            if(ratesFromProxy == null)
            {
                _logger.LogError("Couldn't retrieve the rates for currency conversion from proxy nor redis.");
                result.AddError(1, "Couldn't get the rates from proxy nor redis");
                _redisCacheClient.Set("RateList", ratesFromProxy);
                return result;
            }
            _logger.LogInformation("Retrieved rates from Proxy.");
            result.SetResult(ratesFromProxy);
            return result;
        }
    }
}

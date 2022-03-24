using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using VY.GNB.Dtos;
using VY.GNB.XCutting.Contracts.Services;

namespace VY.GNB.XCutting.Implementation.Services
{
    public class TransactionsProxy : IProxy<TransactionDto>
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public TransactionsProxy(IConfiguration configuration,
                          HttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<TransactionDto>> GetAsync()
        {
            List<TransactionDto> rates = new List<TransactionDto>();
            using (var request = new HttpRequestMessage(HttpMethod.Get, _configuration.GetSection("TransactionsUri").Value))
            {
                using (var response = await _httpClient.SendAsync(request))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var res = JsonSerializer.Deserialize<IEnumerable<TransactionDto>>(content);
                        rates.AddRange(res);
                    }
                }
            }
            return rates;
        }
    }
}

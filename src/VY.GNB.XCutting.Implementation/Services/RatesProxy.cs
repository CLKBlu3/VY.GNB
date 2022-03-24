using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using VY.GNB.Dtos;
using VY.GNB.XCutting.Contracts.Services;

namespace VY.GNB.XCutting.Implementation.Services
{
    public class RatesProxy : IProxy<RateDto>
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public RatesProxy(IConfiguration configuration,
                          HttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<RateDto>> GetAsync()
        {
            List<RateDto> rates = new List<RateDto>();
            using (var request = new HttpRequestMessage(HttpMethod.Get, _configuration.GetSection("RatesUri").Value))
            {
                using (var response = await _httpClient.SendAsync(request))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var res = JsonSerializer.Deserialize<IEnumerable<RateDto>>(content);
                        rates.AddRange(res);
                    }
                }
            }
            return rates;
        }

    }
}

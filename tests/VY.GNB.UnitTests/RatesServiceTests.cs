using Microsoft.Extensions.Configuration;
using Moq;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using VY.GNB.Dtos;
using VY.GNB.XCutting.Contracts.Services;
using VY.GNB.XCutting.Implementation.Services;
using Xunit;

namespace VY.GNB.UnitTests
{
    public class RatesServiceTests
    {
        private readonly IConfiguration _configuration;

        public RatesServiceTests(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [Fact]
        public async Task GetAsyncCorrectUri_ReturnsRatesDataAsync()
        {
            IProxy<RateDto> proxy = new RatesProxy(_configuration, new HttpClient());
            var res = await proxy.GetAsync();
            Assert.True(res.Any());
        }

        [Fact]
        public async Task GetAsyncWrongUri_ReturnsRatesDataAsync()
        {
            Mock<IConfiguration> mockedConf = new Mock<IConfiguration>();
            mockedConf.Setup(x => x.GetSection(It.IsAny<string>()).ToString()).Returns("http://google.com");
            IProxy<RateDto> proxy = new RatesProxy(mockedConf.Object, new HttpClient());
            var res = await proxy.GetAsync();
            Assert.False(res.Any());
        }
    }
}

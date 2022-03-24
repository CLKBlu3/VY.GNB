using Microsoft.Extensions.DependencyInjection;
using VY.GNB.Dtos;
using VY.GNB.XCutting.Contracts.Services;
using VY.GNB.XCutting.Implementation.Services;

namespace VY.GNB.XCutting.Implementation.Extensions
{
    public static class XCuttingExtensions
    {
        public static IServiceCollection AddXCuttingServices(this IServiceCollection services)
        {
            services.AddTransient<IProxy<RateDto>, RatesProxy>();
            services.AddTransient<IProxy<TransactionDto>, TransactionsProxy>();
            return services;
        }
    }
}

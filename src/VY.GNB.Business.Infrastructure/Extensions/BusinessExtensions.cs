using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VY.GNB.Business.Contracts.Services;
using VY.GNB.Business.Implementation.Mapping_Profiles;
using VY.GNB.Business.Implementation.Services;
using VY.GNB.Infrastructure.Implementation.Extensions;
using VY.GNB.XCutting.Implementation.Extensions;

namespace VY.GNB.Business.Implementation.Extensions
{
    public static class BusinessExtensions
    {
        public static IServiceCollection AddBusinessServices(this IServiceCollection services, 
                                                             IConfiguration configuration)
        {
            services.AddAutoMapper(typeof(TransactionProfile),
                                   typeof(RatesProfile));

            services.AddTransient<IRatesService, RatesService>();
            services.AddTransient<ITransactionsService, TransactionsService>();
            services.AddTransient<IEurConverter, EurConverterService>();

            services.AddInfrastructureServices(configuration);
            services.AddXCuttingServices();
            services.AddRedisClientsFromConfiguration(configuration);

            return services;
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VY.GNB.Infrastructure.Contracts.Entities;
using VY.GNB.Infrastructure.Contracts.Repositories;
using VY.GNB.Infrastructure.Implementation.Context;
using VY.GNB.Infrastructure.Implementation.Repositories;

namespace VY.GNB.Infrastructure.Implementation.Extensions
{
    public static class InfrastructureExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, 
                                                                   IConfiguration configuration)
        {
            services.AddDbContext<TransactionsContext>(c =>
            {
                c.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
                c.UseSqlServer(configuration.GetConnectionString("Database"));
            });

            services.AddTransient<IRepository<Transaction>, TransactionRepository>();

            return services;
        }
    }
}

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VY.GNB.Infrastructure.Implementation.Context;

namespace VY.GNB.IntegrationTests
{
    public class InMemoryFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class 
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);
            builder.ConfigureServices(services =>
            {
                var dbDescriptor = services.SingleOrDefault(c => c.ServiceType == typeof(DbContextOptions<TransactionsContext>));
            });
        }
    }
}

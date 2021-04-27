using BenchmarkDotNet.Attributes;
using BlazorEFCoreMultitenant.Data;
using BlazorEFCoreMultitenant.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContextFactoryBenchmarks
{
    [SimpleJob]
    public class DbContextFactoryBenchmark
    {
        private IServiceProvider serviceProvider;

        [GlobalSetup]
        public void Setup()
        {
            var collection = new ServiceCollection();
            collection.AddScoped<TenantProvider>();
            collection.AddDbContextFactory<SingleDbContext>(lifetime: ServiceLifetime.Scoped);
            collection.AddDbContextFactory<MultipleDbContext>(lifetime: ServiceLifetime.Transient);
            var provider = collection.BuildServiceProvider();
            serviceProvider = provider.CreateScope().ServiceProvider;
        }

        [Benchmark]
        public SingleDbContext SingleContext()
        {
            var factory = serviceProvider.GetService<IDbContextFactory<SingleDbContext>>();
            return factory.CreateDbContext();
        }

        [Benchmark]
        public MultipleDbContext MultipleContext()
        {
            var factory = serviceProvider.GetService<IDbContextFactory<MultipleDbContext>>();
            return factory.CreateDbContext();
        }
    }
}

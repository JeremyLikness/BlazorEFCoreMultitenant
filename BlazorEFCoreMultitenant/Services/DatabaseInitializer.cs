using BlazorEFCoreMultitenant.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace BlazorEFCoreMultitenant.Services
{
    public class DatabaseInitializer
    {
        private readonly IDbContextFactory<SingleDbContext> singleFactory;
        private readonly IDbContextFactory<MultipleDbContext> multipleFactory;
        private readonly TenantProvider tenantProvider;

        public DatabaseInitializer(
            TenantProvider tenantProvider,
            IDbContextFactory<SingleDbContext> singleFactory,
            IDbContextFactory<MultipleDbContext> multipleFactory)
        {
            this.tenantProvider = tenantProvider;
            this.singleFactory = singleFactory;
            this.multipleFactory = multipleFactory;
        }
        public async Task InitializeAsync()
        {
            var types = TypeProvider.GetTypes();
            var seedData = types.SelectMany(t => t.GetMethods(
                BindingFlags.Public | 
                BindingFlags.Instance |
                BindingFlags.Static))
                .Select(m => new DataMethod(m)).ToList();
            // check the single database
            using var single = singleFactory.CreateDbContext();
            if (await single.Database.EnsureCreatedAsync())
            {
                single.Methods.AddRange(seedData);
                //single.Parameters.AddRange(seedData.SelectMany(m => m.Parameters));
                await single.SaveChangesAsync();
            }

            // iterate through individual databases
            var currentTenant = tenantProvider.GetTenant();

            foreach (var type in TypeProvider.GetTypes())
            {
                var tenant = type.FullName;
                tenantProvider.SetTenant(tenant);
                using var multiple = multipleFactory.CreateDbContext();
                if (await multiple.Database.EnsureCreatedAsync())
                {
                    multiple.Methods.AddRange(seedData.Where(sd => sd.ParentType == tenant));
                    //multiple.Parameters.AddRange(seedData.SelectMany(m => m.Parameters));
                    await multiple.SaveChangesAsync();
                }
            }

            tenantProvider.SetTenant(currentTenant);
        }
    }
}

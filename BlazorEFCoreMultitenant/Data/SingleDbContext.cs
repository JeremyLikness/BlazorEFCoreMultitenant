using BlazorEFCoreMultitenant.Services;
using Microsoft.EntityFrameworkCore;

namespace BlazorEFCoreMultitenant.Data
{
    public class SingleDbContext : DbContext, ICustomerContext
    {
        private readonly string tenant = string.Empty;

        public SingleDbContext(
            DbContextOptions<SingleDbContext> options,
            TenantProvider tenantProvider)
            : base(options) 
        {
            tenant = tenantProvider.GetTenant();
        }

        public DbSet<DataMethod> Methods { get; set; }

        public DbSet<DataParameter> Parameters { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DataMethod>()
                .HasMany(dm => dm.Parameters)
                .WithOne();

            modelBuilder.Entity<DataMethod>()
                .HasQueryFilter(dm => dm.ParentType == tenant);

            base.OnModelCreating(modelBuilder);
        }
    }
}

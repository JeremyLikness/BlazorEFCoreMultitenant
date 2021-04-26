using BlazorEFCoreMultitenant.Services;
using Microsoft.EntityFrameworkCore;

namespace BlazorEFCoreMultitenant.Data
{
    public class MultipleDbContext : DbContext, ICustomerContext
    {
        private readonly string tenant = string.Empty;
        public MultipleDbContext(
            DbContextOptions<MultipleDbContext> options,
            TenantProvider tenantProvider)
            : base(options) 
        {
            tenant = tenantProvider.GetTenantShortName();
        }

        public DbSet<DataMethod> Methods { get; set; }

        public DbSet<DataParameter> Parameters { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={tenant}.sqlite");
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DataMethod>()
                .HasMany(dm => dm.Parameters)
                .WithOne();

            base.OnModelCreating(modelBuilder);
        }
    }
}

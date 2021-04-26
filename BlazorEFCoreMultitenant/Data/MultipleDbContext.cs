using BlazorEFCoreMultitenant.Services;
using Microsoft.EntityFrameworkCore;

namespace BlazorEFCoreMultitenant.Data
{
    public class MultipleDbContext : DbContext, ICustomerContext
    {
        public MultipleDbContext(
            DbContextOptions<MultipleDbContext> options)
            : base(options) 
        {
        }

        public DbSet<DataMethod> Methods { get; set; }

        public DbSet<DataParameter> Parameters { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DataMethod>()
                .HasMany(dm => dm.Parameters)
                .WithOne();

            base.OnModelCreating(modelBuilder);
        }
    }
}

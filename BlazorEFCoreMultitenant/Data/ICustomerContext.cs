using Microsoft.EntityFrameworkCore;

namespace BlazorEFCoreMultitenant.Data
{
    public interface ICustomerContext
    {
        DbSet<DataMethod> Methods { get; }
        DbSet<DataParameter> Parameters { get; }
    }
}

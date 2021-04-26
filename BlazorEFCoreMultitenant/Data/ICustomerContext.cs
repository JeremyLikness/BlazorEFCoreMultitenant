using Microsoft.EntityFrameworkCore;
using System;

namespace BlazorEFCoreMultitenant.Data
{
    public interface ICustomerContext : IDisposable
    {
        DbSet<DataMethod> Methods { get; }
        DbSet<DataParameter> Parameters { get; }
    }
}

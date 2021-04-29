# Blazor EF Core Multitenant

Examples of using multi-tenancy with Entity Framework Core in a Blazor app using the data context factory.

Read the related, detailed blog post [here](https://blog.jeremylikness.com/blog/multitenancy-with-ef-core-in-blazor-server-apps/).

## Quickstart

1. Clone the repository: `git clone https://github.com/JeremyLikness/BlazorEFCoreMultitenant`
1. Launch the app
    1. Open in VS Code and hit F5, or
    1. Open in Visual Studio and hit F5, or
    1. From a command line at the root of the project, type `dotnet run`
1. The databases ship with the project. If you need to create them or wish to recreate them, delete the `.sqlite` files then use the button on the main page of the app to regenerate them.
1. Select your tenant and navigate to the multidatabase or single database solutions.

The database simply tracks methods and parameters. The `ParentType` is used for the "tenant."

## Notes

This example shows two different solutions. Both configure the context in a way that makes the tenancy ambient to the consumer. The code simply loads the list of `DataMethod` 
instances without filters, but only the tenant-specific items are returned. This is because the single database approach uses a global query filter and the multiple database
approach uses a differnet database altogether for each tenant.

### Single Database

For a single database, the column to hold tenant in this example is `ParentType`. It could be a `tenantId` or some other field. The factory is set to `Scoped` instead of singleton so 
it can acquire the instance of `TenantProvider` that is running for the current user. This will cache the options, which is fine for a single database. It also caches the instance of 
of the `TenantProvider`. This is the same one use in the rest of the application, so the `OnModelBuilding` override is able to add a global filter based on the current tenant.

### Multiple Databases

For multiple databases, the database is named after the tenant so there is a one-to-one mapping. Instead of using a global filter, the tenant is used to build the connection string
and passed back. To allow the case when a user might change their tenant "on the fly" the factory is scoped as `Transient` instead so the tenant is always re-evaluated in case it
changes.

## Performance

You can use the benchmark project to evaluate performance. Be sure to run it in `Release` mode:

```bash
dotnet run -c Release
```

Here are the results on my laptop:

|          Method |     Mean |     Error |    StdDev |
|---------------- |---------:|----------:|----------:|
|   SingleContext | 1.134 us | 0.0224 us | 0.0249 us |
| MultipleContext | 3.087 us | 0.0608 us | 0.0964 us |

Although the `Transient` is slower than `Scoped`, it is still a fast operation.
Another way to think about 3us is 333k requests per second.

---

Feedback? Questions? Message me on Twitter: [@JeremyLikness](https:///www.twitter.com/jeremylikness)

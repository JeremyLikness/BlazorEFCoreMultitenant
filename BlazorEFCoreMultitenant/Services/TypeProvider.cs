using System;
using System.Collections.Generic;

namespace BlazorEFCoreMultitenant.Services
{
    public static class TypeProvider
    {
        public static List<Type> GetTypes() =>
            new ()
            {
                typeof(Program),
                typeof(Startup),
                typeof(TenantProvider)
            };
    }
}

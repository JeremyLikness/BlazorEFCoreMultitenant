using System;
using System.Collections.Generic;
using System.Linq;

namespace BlazorEFCoreMultitenant.Services
{
    public class TenantProvider
    {
        private readonly IDictionary<Guid, Action> callbacks = new Dictionary<Guid, Action>();

        private string tenant = TypeProvider.GetTypes().First().FullName;

        public void SetTenant(string tenant)
        {
            this.tenant = tenant;
            foreach (var callback in callbacks.Values)
            {
                callback();
            }
        }

        public void Unregister(Guid id)
        {
            if (callbacks.ContainsKey(id))
            {
                callbacks.Remove(id);
            }
        }

        public Guid Register(Action callback)
        {
            var id = Guid.NewGuid();
            callbacks.Add(id, callback);
            return id;
        }

        public string GetTenant() => tenant;

        public string GetTenantShortName() => tenant.Split('.')[^1];
    }
}

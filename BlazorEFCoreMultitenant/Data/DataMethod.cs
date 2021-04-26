using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace BlazorEFCoreMultitenant.Data
{
    public class DataMethod
    {
        public DataMethod() { }

        public DataMethod(MethodInfo method)
        {
            Name = method.Name;
            ReturnType = method.ReturnType.FullName;
            ParentType = method.DeclaringType.FullName;
            
            Parameters = method.GetParameters()
                .Select(p => new DataParameter(p))
                .ToList();

            foreach (var parameter in Parameters)
            {
                parameter.Method = this;
            }
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string ReturnType { get; set; }

        public string ParentType { get; set; }

        public IList<DataParameter> Parameters { get; set; } =
            new List<DataParameter>();
    }
}

using System.Reflection;

namespace BlazorEFCoreMultitenant.Data
{
    public class DataParameter
    {
        public DataParameter() { }

        public DataParameter(ParameterInfo parameter)
        {
            Name = parameter.Name;
            Type = parameter.ParameterType.FullName;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public DataMethod Method { get; set; }
    }
}

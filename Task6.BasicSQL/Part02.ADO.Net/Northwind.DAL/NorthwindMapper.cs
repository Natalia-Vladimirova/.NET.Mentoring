using System;
using System.Data;
using System.Linq;
using System.Reflection;
using Fasterflect;
using Northwind.DAL.Interfaces;

namespace Northwind.DAL
{
    public class NorthwindMapper : IMapper
    {
        public T Map<T>(IDataReader reader) where T : new()
        {
            T obj = new T();

            var dataColumnNames = Enumerable
                .Range(0, reader.FieldCount)
                .Select(reader.GetName)
                .ToList();

            var typeProperties = typeof(T)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in typeProperties)
            {
                var existingName = dataColumnNames
                    .FirstOrDefault(x => string.Equals(x, property.Name, StringComparison.OrdinalIgnoreCase));

                if (existingName == null) continue;
                
                try
                {
                    obj.TrySetValue(property.Name, reader[existingName]);
                }
                catch (InvalidCastException)
                {
                    var defaultValue = this.CallMethod(new[] { property.PropertyType }, "GetDefault");
                    obj.TrySetValue(property.Name, defaultValue);
                }
            }

            return obj;
        }

        private T GetDefault<T>()
        {
            return default(T);
        }
    }
}

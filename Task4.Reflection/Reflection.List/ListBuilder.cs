using System;
using System.Collections.Generic;
using Fasterflect;
using Reflection.Base;

namespace Reflection.List
{
    public class ListBuilder : BaseBuilder
    {
        private readonly Type _instanceType;
        
        public ListBuilder(Type type) 
        {
            _instanceType = type;
        }

        public ListBuilder(string type)
        {
            _instanceType = GetTypeArgument(type);
        }
        
        public object List => _instance;

        protected override Type GetGenericTypeDefinition()
        {
            return typeof(List<>);
        }

        protected override object CreateInstanceOfGenericType(Type genericTypeDefinition)
        {
            return genericTypeDefinition
                .MakeGenericType(_instanceType)
                .CreateInstance();
        }
        
        public void AddValue(object value)
        {
            CheckParameter(nameof(value), value, _instanceType);

            _instance.CallMethod("Add", new[] {_instanceType}, value);
        }
    }
}

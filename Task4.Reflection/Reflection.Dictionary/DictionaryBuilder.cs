using System;
using System.Collections.Generic;
using Fasterflect;
using Reflection.Base;

namespace Reflection.Dictionary
{
    public class DictionaryBuilder : BaseBuilder
    {
        private readonly Type _keyType;
        private readonly Type _valueType;
        
        public DictionaryBuilder(Type keyType, Type valueType)
        {
            _keyType = keyType;
            _valueType = valueType;
        }

        public DictionaryBuilder(string keyType, string valueType)
        {
            _keyType = GetTypeArgument(keyType);
            _valueType = GetTypeArgument(valueType);
        }

        public object Instance => _instance;

        protected override Type GetGenericTypeDefinition()
        {
            return typeof(Dictionary<,>);
        }

        protected override object CreateInstanceOfGenericType(Type genericTypeDefinition)
        {
            return genericTypeDefinition
                .MakeGenericType(_keyType, _valueType)
                .CreateInstance();
        }
        
        public void AddValue(object key, object value)
        {
            CheckParameter(nameof(key), key, _keyType);
            CheckParameter(nameof(value), value, _valueType);

            _instance.CallMethod("Add", new[] {_keyType, _valueType}, key, value);
        }
    }
}

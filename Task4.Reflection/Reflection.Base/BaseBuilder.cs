using System;
using Fasterflect;

namespace Reflection.Base
{
    public abstract class BaseBuilder
    {
        protected object _instance;
        
        public void CreateInstance()
        {
            var genericTypeDefinition = GetGenericTypeDefinition();
            _instance = CreateInstanceOfGenericType(genericTypeDefinition);
        }

        protected abstract Type GetGenericTypeDefinition();

        protected abstract object CreateInstanceOfGenericType(Type genericTypeDefinition);

        protected Type GetTypeArgument(string type)
        {
            return Type.GetType(type, true);
        }

        protected void CheckParameter(string argName, object value, Type type)
        {
            if (value == null)
            {
                var defaultValue = this.CallMethod(new[] { type }, "GetDefault");

                if (defaultValue != null)
                {
                    throw new ArgumentException($"Argument {argName} cannot be null");
                }
            }
            else if (value.GetType() != type &&
                !value.GetType().InheritsOrImplements(type))
            {
                throw new ArgumentException($"Argument {argName} does not match the dictionary type");
            }
        }

        private T GetDefault<T>()
        {
            return default(T);
        }
    }
}

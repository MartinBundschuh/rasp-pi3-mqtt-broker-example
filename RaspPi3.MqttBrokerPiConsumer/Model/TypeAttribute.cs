using System;
using System.Collections.Generic;
using System.Reflection;

namespace RaspPi3.MqttBrokerPiConsumer.Model
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    internal sealed class TypeAttribute : Attribute
    {
        internal Type Type { get; private set; }

        internal TypeAttribute(Type type)
        {
            Type = type;
        }

        internal static IEnumerable<Type> GetTypeAttributes()
        {
            var assembly = typeof(TypeAttribute).GetTypeInfo().Assembly;

            foreach (var type in assembly.GetTypes())
            {
                var returnType = type.GetTypeInfo().GetCustomAttribute<TypeAttribute>();
                if (returnType != null)
                    yield return returnType.Type;
            }
        }
    }
}

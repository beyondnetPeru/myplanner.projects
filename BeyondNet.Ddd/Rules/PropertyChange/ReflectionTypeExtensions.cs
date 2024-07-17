
using System.Reflection;

namespace BeyondNet.Ddd.Rules.PropertyChange
{
    internal static class ReflectionTypeExtensions
    {

        internal static bool GetIsValueType(this Type type)
        {
            return type.GetTypeInfo().IsValueType;
        }

        internal static bool GetIsAssignableFrom(this Type type, Type secondType)
        {
            return type.GetTypeInfo().IsAssignableFrom(secondType.GetTypeInfo());
        }
    }
}

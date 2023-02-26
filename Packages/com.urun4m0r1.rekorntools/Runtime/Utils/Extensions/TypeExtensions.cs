#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Urun4m0r1.RekornTools.Utils
{
    public static class TypeExtensions
    {
        public static Type GetUnderlyingType(this MemberInfo? member)
        {
            var type = member?.MemberType switch
            {
                MemberTypes.Event    => ((EventInfo)member).EventHandlerType,
                MemberTypes.Field    => ((FieldInfo)member).FieldType,
                MemberTypes.Method   => ((MethodInfo)member).ReturnType,
                MemberTypes.Property => ((PropertyInfo)member).PropertyType,
                _ => throw new ArgumentException(
                    "Input MemberInfo must be if type EventInfo, FieldInfo, MethodInfo, or PropertyInfo"),
            };
            return type.GetNullableUnderlyingType() ?? type;
        }

        public static Type? GetNullableUnderlyingType(this Type type)
        {
            if (!type.IsGenericType) return null;
            if (type.GetGenericTypeDefinition() != typeof(Nullable<>)) return null;

            return type.GetGenericArguments()[0];
        }

        public static void SetMemberVariable(this MemberInfo member, object? target, object? value)
        {
            switch (member)
            {
                case FieldInfo fieldInfo:
                    fieldInfo.SetValue(target, value);
                    break;
                case PropertyInfo propertyInfo:
                    propertyInfo.SetValue(target, value);
                    break;
                default:
                    throw new ArgumentException("Input MemberInfo must be if type FieldInfo or PropertyInfo");
            }
        }

        public static IEnumerable<MemberInfo> GetVariableMembers(this IReflect type, BindingFlags flags)
        {
            var fields     = type.GetFields(flags);
            var properties = type.GetProperties(flags);
            return fields.Concat<MemberInfo>(properties);
        }

        public static MemberInfo GetVariableMember(this IReflect type, string name, BindingFlags flags)
        {
            var field    = type.GetField(name, flags);
            var property = type.GetProperty(name, flags);
            return (MemberInfo)field ?? property;
        }

        public static bool IsNumericParsable(this Type? type)
        {
            if (type == null) return false;

            type = GetNullableType(type);

            return type.IsPrimitive        ||
                   type == typeof(decimal) ||
                   type == typeof(Guid);
        }

        public static bool IsStringParsable(this Type? type)
        {
            if (type == null) return false;

            type = GetNullableType(type);

            return type == typeof(string)         ||
                   type == typeof(char)           ||
                   type == typeof(DateTime)       ||
                   type == typeof(DateTimeOffset) ||
                   type == typeof(TimeSpan);
        }

        private static Type GetNullableType(Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                type = type.GetGenericArguments()[0];
            return type;
        }

        public static bool IsDictionary(this Type? type)
        {
            if (type == null) return false;

            type = GetNullableType(type);

            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>);
        }
    }
}

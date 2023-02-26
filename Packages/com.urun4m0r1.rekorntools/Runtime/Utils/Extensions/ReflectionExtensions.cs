#nullable enable

using System;
using System.Reflection;
using System.Text;
using JetBrains.Annotations;

namespace Urun4m0r1.RekornTools.Utils
{
    public static class ReflectionExtensions
    {
#region Attribute
        public static readonly BindingFlags Everything = ~BindingFlags.Default;

        public static (MemberInfo, Type) GetFieldOrProperty(this Type? type, string? name)
        {
            if (string.IsNullOrWhiteSpace(name)) return (null, type);

            var fieldInfo = type?.GetField(name, Everything);
            if (fieldInfo != null) return (fieldInfo, fieldInfo.FieldType);

            var propertyInfo = type?.GetProperty(name, Everything);
            if (propertyInfo != null) return (propertyInfo, propertyInfo.PropertyType);

            return (null, type);
        }
#endregion // Attribute

#region Property
        static readonly StringBuilder Sb = new StringBuilder();

        [NotNull] const string AutoPropertyHeader = "<";
        [NotNull] const string AutoPropertyFooter = ">k__BackingField";

        public static bool IsAutoProperty(string name) =>
            name.StartsWith(AutoPropertyHeader, StringComparison.Ordinal)
         && name.EndsWith(AutoPropertyFooter, StringComparison.Ordinal);

        public static string ResolveDisplayName(string name)
        {
            if (!IsAutoProperty(name)) return name;

            name = name.Remove(0,                                       AutoPropertyHeader.Length);
            name = name.Remove(name.Length - AutoPropertyFooter.Length, AutoPropertyFooter.Length);

            return name;
        }

        public static string ResolveFieldName(string name)
        {
            if (IsAutoProperty(name)) return name;

            Sb.Clear();
            Sb.Append(AutoPropertyHeader);
            Sb.Append(name);
            Sb.Append(AutoPropertyFooter);
            return Sb.ToString();
        }
#endregion // Property
    }
}

#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json;
using Urun4m0r1.RekornTools.Utils;

namespace Urun4m0r1.RekornTools.Data
{
    public static class DataDeserializer
    {
        private const BindingFlags MemberFlags = BindingFlags.Public           |
                                                 BindingFlags.NonPublic        |
                                                 BindingFlags.Static           |
                                                 BindingFlags.FlattenHierarchy |
                                                 BindingFlags.Instance         |
                                                 BindingFlags.GetField         |
                                                 BindingFlags.GetProperty;

        #region CsvDeserializer

        public static IEnumerable<T> DeserializeCsv<T>(string? source) where T : new()
        {
            string str = source.ThrowIfWhiteSpace().Trim();

            string?[]?[]? rawData = str.SplitByLineFeed()?.Select(row => row?.Split(',')).ToArray();
            if (rawData?.Length < 2 || (rawData?.Any(col => col?.Length < 1) ?? true))
                throw new DataParserException("File has no data.");

            rawData = rawData.Select(col => col?.Select(elem => elem?.Trim() ?? "").ToArray()).ToArray();

            var members = typeof(T).GetVariableMembers(MemberFlags).ToArray();
            var fields  = rawData[0]!.Select(ParseField).ToArray();
            if (fields.All(x => x == null)) throw new DataParserException("File header is invalid.");

            var data = rawData.Skip(1).ToArray();
            return ParseCsvRow<T>(data!, fields);

            MemberInfo? ParseField(string? fieldName) =>
                (from x in members
                    let name = x.GetCustomAttribute<ParseAsAttribute>()?.Name ?? x.Name
                    let ignore = x.GetCustomAttribute<ParseIgnoreAttribute>() != null
                    where !ignore && name == fieldName
                    select x).FirstOrDefault();
        }


        private static IEnumerable<T> ParseCsvRow<T>(IReadOnlyList<string[]> data, IReadOnlyList<MemberInfo?> members)
            where T : new()
        {
            var rows = new T[data.Count];

            for (var row = 0; row < rows.Length; row++)
                rows[row] = DeserializeCsvRow<T>(data[row], members);

            return rows;
        }

        private static T DeserializeCsvRow<T>(IReadOnlyList<string> data, IReadOnlyList<MemberInfo?> members)
            where T : new()
        {
            object row = new T();

            for (var col = 0; col < data.Count; col++)
            {
                if (members[col] == null) continue;

                var value = ParseCsvElement(data[col], members[col].GetUnderlyingType());
                members[col]?.SetMemberVariable(row, value);
            }

            return (T)row;
        }

        private static object? ParseCsvElement(string element, Type type)
        {
            if (element.Equals("") || element.Equals("null")) return null;

            return type switch
            {
                { } t when t.IsStringParsable()  => element,
                { } t when t.IsNumericParsable() => Convert.ChangeType(element, type),
                { IsEnum: true }                 => DeserializeEnum(element, type),
                _                                => throw new DataParserException($"Unsupported type: {type.Name}")
            };
        }

        #endregion

        #region JsonDeserializer

        public static T DeserializeJson<T>(string? source) where T : new()
        {
            var str         = source.ThrowIfWhiteSpace().Trim();
            var rootElement = JsonDocument.Parse(str).RootElement;
            if (rootElement.ValueKind != JsonValueKind.Object)
                throw new DataParserException("Root element is not an object.");

            return (T)(DeserializeJsonElement(rootElement, typeof(T)) ??
                       throw new DataParserException("Root element is not of type T."));
        }

        private static object? DeserializeJsonElement(JsonElement element, Type type)
        {
            var box = FormatterServices.GetUninitializedObject(type);

            var members = type.GetVariableMembers(MemberFlags).ToArray();
            foreach (var member in members)
            {
                if (member.GetCustomAttribute<ParseIgnoreAttribute>() != null) continue;

                var memberName = member.GetCustomAttribute<ParseAsAttribute>()?.Name ?? member.Name;
                if (element.TryGetProperty(memberName, out var chileElement))
                {
                    var value = ParseJsonElement(chileElement, member.GetUnderlyingType());
                    member.SetMemberVariable(box, value);
                }
            }

            return box;
        }

        private static object? ParseJsonElement(JsonElement element, Type type)
        {
            if (element.ValueKind == JsonValueKind.Null) return null;

            return type switch
            {
                { } t when t.IsStringParsable()  => element.GetString(),
                { } t when t.IsNumericParsable() => Convert.ChangeType(element.GetRawText(), type),
                { } t when t.IsDictionary()      => DeserializeJsonDictionary(element, type),
                { IsEnum     : true }            => DeserializeEnum(element.GetString(), type),
                { IsArray    : true }            => DeserializeJsonArray(element, type),
                { IsValueType: true }            => DeserializeJsonElement(element, type),
                { IsClass    : true }            => DeserializeJsonElement(element, type),
                _                                => throw new DataParserException($"Unsupported type: {type.Name}"),
            };
        }

        private static object DeserializeJsonArray(JsonElement arrayElement, Type arrayType)
        {
            var elementType = arrayType.GetElementType() ??
                              throw new DataParserException($"{arrayType} is not an array.");
            var elements = arrayElement.EnumerateArray().ToArray();

            var arrayBox = Array.CreateInstance(elementType, elements.Length);

            for (var i = 0; i < elements.Length; i++)
            {
                var value = ParseJsonElement(elements[i], elementType);
                arrayBox.SetValue(value, i);
            }

            return arrayBox;
        }

        private static object DeserializeJsonDictionary(JsonElement dictElement, Type dictType)
        {
            var keyType   = dictType.GenericTypeArguments[0];
            var valueType = dictType.GenericTypeArguments[1];

            var dictBox = Activator.CreateInstance(dictType);

            foreach (var element in dictElement.EnumerateObject())
            {
                var key   = keyType switch
                {
                    { IsEnum: true } => DeserializeEnum(element.Name, keyType),
                    _ => element.Name,
                };

                var value = ParseJsonElement(element.Value, valueType);

                dictBox.GetType().GetMethod("Add")?.Invoke(dictBox, new[] { key, value });
            }

            return dictBox;
        }

        #endregion

        private static object? DeserializeEnum(string? element, Type type)
        {
            if (string.IsNullOrWhiteSpace(element)) return null;

            try
            {
                return Enum.Parse(type, element);
            }
            catch (ArgumentException)
            {
                return Enum.GetValues(type).GetValue(0);
            }
        }
    }
}

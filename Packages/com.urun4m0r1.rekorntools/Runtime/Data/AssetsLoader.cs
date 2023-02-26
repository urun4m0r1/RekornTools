#nullable enable

using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Urun4m0r1.RekornTools.Data
{
    public static class AssetsLoader
    {
        public static bool TryLoadCsv<T>(string path, out IEnumerable<T> data) where T : new()
        {
            try
            {
                data = LoadCsv<T>(path);
                return true;
            }
            catch (DataParserException e)
            {
                Debug.LogWarning(e);
                data = new T[] { };
                return false;
            }
        }

        public static bool TryLoadJson<T>(string path, out T data) where T : new()
        {
            try
            {
                data = LoadJson<T>(path);
                return true;
            }
            catch (DataParserException e)
            {
                Debug.LogWarning(e);
                data = new T();
                return false;
            }
        }

        public static IEnumerable<T> LoadCsv<T>(string path) where T : new() => DataDeserializer.DeserializeCsv<T>(LoadText(path));

        [NotNull] public static T LoadJson<T>(string path) where T : new() => DataDeserializer.DeserializeJson<T>(LoadText(path));

        public static string? LoadText(string path)
        {
            var textAsset = Resources.Load<TextAsset>(path);
            if (textAsset == null) throw new DataParserException($"Cannot find asset with path: {path}");

            return textAsset.text;
        }
    }
}

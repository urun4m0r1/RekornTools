﻿#nullable enable

using UnityEditor;
using UnityEditor.Presets;
using UnityEngine;

namespace Urun4m0r1.RekornTools.Serialization
{
    public static class AssetHelper
    {
        public static void ApplyPreset(Object? obj, Preset preset)
        {
            var importer = GetAssetImporter(obj);
            if (importer == null) return;

            preset.ApplyTo(importer);
            importer.SaveAndReimport();
        }

        public static AssetImporter? GetAssetImporter(Object? obj)
        {
            if (obj == null) return null;

            var path = AssetDatabase.GetAssetPath(obj);
            return AssetImporter.GetAtPath(path);
        }
    }
}

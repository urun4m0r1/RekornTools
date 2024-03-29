﻿#nullable enable

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Urun4m0r1.RekornTools.Utils
{
    public static class ShaderPropertyExtensions
    {
        public static IEnumerable<Shader>? AllUserShadersInProject =>
            from material in AllMaterialsInProject
            let shader = material.shader
            where shader != null
            select shader;

        public static IEnumerable<Material>? AllMaterialsInProject =>
#if UNITY_EDITOR
            from guid in AssetDatabase.FindAssets("t:Material")
            let path = AssetDatabase.GUIDToAssetPath(guid)
            let material = AssetDatabase.LoadAssetAtPath<Material>(path)
            where material != null
            select material;
#else
            null;
#endif

        public static IEnumerable<Shader>? AllShadersInProject =>
#if UNITY_EDITOR
            from guid in AssetDatabase.FindAssets("t:Shader")
            let path = AssetDatabase.GUIDToAssetPath(guid)
            let shader = AssetDatabase.LoadAssetAtPath<Shader>(path)
            where shader != null
            select shader;
#else
            null;
#endif
    }
}

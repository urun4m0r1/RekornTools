﻿#if UNITY_EDITOR
#nullable enable

using System;
using UnityEditor;
using UnityEngine;

namespace Urun4m0r1.RekornTools.Unity
{
    public static class GizmoUtils
    {
        public static float ZoomLevel
        {
            get
            {
                if (SceneView.currentDrawingSceneView) return SceneView.currentDrawingSceneView.size;

                return 1f;
            }
        }
        public static float InversedZoomLevel => 1f / ZoomLevel;

        public static float ClampedZoomLevel(float         farDistance) => Mathf.Clamp(ZoomLevel, 1f, farDistance);
        public static float ClampedInversedZoomLevel(float farDistance) => 1f / ClampedZoomLevel(farDistance);

        public static void DrawThickLine(Vector3 start, Vector3 end, float thickness, Color color) =>
            Handles.DrawBezier(start, end, start, end, color, null, thickness);
    }

    [Flags] public enum TransformTool
    {
        None   = 0,
        Move   = 1 << 0,
        Rotate = 1 << 1,
        Scale  = 1 << 2,
    }

    public enum DrawMode
    {
        Invisible,
        OnSelected,
        OnNonSelected,
        Always,
    }
}
#endif

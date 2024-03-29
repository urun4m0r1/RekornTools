﻿#nullable enable

using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace Urun4m0r1.RekornTools.Unity
{
    [DisallowMultipleComponent]
    public class PositionChainGizmos : MonoBehaviour, IGizmos
    {
#if UNITY_EDITOR
        [field: SerializeField]          public  DrawMode        DrawMode { get; set; } = DrawMode.OnSelected;
        [SerializeField]                 private Gradient        _colorGradient  = new();
        [SerializeField, Range(0f, 30f)] private float           _lineThickness  = 5f;
        [SerializeField]                 private bool            _drawDirection  = true;
        [SerializeField]                 private Color           _directionColor = Color.white;
        [SerializeField]                 private List<Transform> _targets        = new();

        [UsedImplicitly] public class GizmosDrawer
        {
            [DrawGizmo(GizmoType.Selected)] private static void OnSelected(PositionChainGizmos t, GizmoType _) => t.DrawOnSelected(DrawGizmos);

            [DrawGizmo(GizmoType.NonSelected)] private static void OnNonSelected(PositionChainGizmos t, GizmoType _) => t.DrawOnNonSelected(DrawGizmos);

            private static void DrawGizmos(PositionChainGizmos target)
            {
                List<Transform> targets = target._targets;
                if (targets.Count < 2) return;

                for (var i = 0; i < targets.Count - 1; i++)
                {
                    Transform t1 = targets[i];
                    Transform t2 = targets[i + 1];

                    if (!t1 || !t2) continue;

                    Vector3   p1 = t1.position;
                    Vector3   p2 = t2.position;

                    Color color = target._colorGradient.Evaluate(i / (targets.Count - 1f));
                    GizmoUtils.DrawThickLine(p1, p2, target._lineThickness, color);

                    if (target._drawDirection) DrawArrow.ForGizmoTwoPoints(p1, p2, target._directionColor);
                }
            }
        }
#endif
    }
}

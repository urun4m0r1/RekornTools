#nullable enable

using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace Urun4m0r1.RekornTools.Unity
{
    [DisallowMultipleComponent]
    public sealed class PositionCenterGizmos : MonoBehaviour, IGizmos
    {
#if UNITY_EDITOR
        [field: SerializeField]          public  DrawMode        DrawMode { get; set; } = DrawMode.OnSelected;
        [SerializeField]                 private Gradient        colorGradient  = new();
        [SerializeField, Range(0f, 30f)] private float           lineThickness  = 5f;
        [SerializeField]                 private bool            drawDirection  = true;
        [SerializeField]                 private Color           directionColor = Color.white;
        [SerializeField]                 private List<Transform> targets        = new();

        [UsedImplicitly] public sealed class GizmosDrawer
        {
            [DrawGizmo(GizmoType.Selected)] private static void OnSelected(PositionCenterGizmos t, GizmoType _) => t.DrawOnSelected(DrawGizmos);

            [DrawGizmo(GizmoType.NonSelected)] private static void OnNonSelected(PositionCenterGizmos t, GizmoType _) => t.DrawOnNonSelected(DrawGizmos);

            private static void DrawGizmos(PositionCenterGizmos target)
            {
                List<Transform> targets = target.targets;
                if (targets.Count < 1) return;

                Vector3 po = target.transform.position;
                for (var i = 0; i < targets.Count; i++)
                {
                    Transform t = targets[i];
                    if (!t) continue;

                    Vector3   p = t.position;

                    var color = target.colorGradient.Evaluate(i / (targets.Count - 1f));
                    GizmoUtils.DrawThickLine(po, p, target.lineThickness, color);

                    if (target.drawDirection) DrawArrow.ForGizmoTwoPoints(po, p, target.directionColor);
                }
            }
        }
#endif
    }
}

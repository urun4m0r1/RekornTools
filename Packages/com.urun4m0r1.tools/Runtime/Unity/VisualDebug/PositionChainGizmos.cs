#nullable enable

using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace Rekorn.Tools.Unity
{
    [DisallowMultipleComponent]
    public class PositionChainGizmos : MonoBehaviour, IGizmos
    {
#if UNITY_EDITOR
        [field: SerializeField] public   DrawMode        DrawMode { get; set; } = DrawMode.OnSelected;
        [SerializeField]                 Gradient        colorGradient  = new();
        [SerializeField, Range(0f, 30f)] float           lineThickness  = 5f;
        [SerializeField]                 bool            drawDirection  = true;
        [SerializeField]                 Color           directionColor = Color.white;
        [SerializeField]                 List<Transform> targets        = new();

        [UsedImplicitly] public class GizmosDrawer
        {
            [DrawGizmo(GizmoType.Selected)]
            static void OnSelected(PositionChainGizmos t, GizmoType _) => t.DrawOnSelected(DrawGizmos);

            [DrawGizmo(GizmoType.NonSelected)]
            static void OnNonSelected(PositionChainGizmos t, GizmoType _) => t.DrawOnNonSelected(DrawGizmos);

            static void DrawGizmos(PositionChainGizmos target)
            {
                List<Transform> targets = target.targets;
                if (targets.Count < 2) return;

                for (var i = 0; i < targets.Count - 1; i++)
                {
                    Transform t1 = targets[i];
                    Transform t2 = targets[i + 1];

                    if (!t1 || !t2) continue;

                    Vector3   p1 = t1.position;
                    Vector3   p2 = t2.position;

                    Color color = target.colorGradient.Evaluate(i / (targets.Count - 1f));
                    GizmoUtils.DrawThickLine(p1, p2, target.lineThickness, color);

                    if (target.drawDirection) DrawArrow.ForGizmoTwoPoints(p1, p2, target.directionColor);
                }
            }
        }
#endif
    }
}

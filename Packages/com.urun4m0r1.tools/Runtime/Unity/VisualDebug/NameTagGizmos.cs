#nullable enable

#if UNITY_EDITOR
using UnityEditor;
#endif

using JetBrains.Annotations;
using Rekorn.Tools.Math;
using UnityEngine;

namespace Rekorn.Tools.Unity
{
    [DisallowMultipleComponent]
    public sealed class NameTagGizmos : MonoBehaviour, IGizmos
    {
#if UNITY_EDITOR
        [field: SerializeField] public DrawMode DrawMode { get; set; } = DrawMode.OnSelected;
        [SerializeField]               string   nameOverride = string.Empty;

        [Header("Font Settings")]
        [SerializeField]                  bool  autoScaled               = true;
        [SerializeField]                  Color textColor                = Color.white;
        [SerializeField, Range(0f, 100f)] int   fontSize                 = 80;
        [SerializeField, Range(1f, 10f)]  float fontSizeClippingDistance = 5f;

        [Header("Position")]
        [SerializeField] bool                        distanceAutoScaled = true;
        [SerializeField] Color                       lineColor          = Color.magenta;
        [SerializeField] VectorExtensions.NormalAxis axis               = VectorExtensions.NormalAxis.Up;

        [SerializeField, Range(0f, 10f)] float distance = 1.5f;

        readonly GUIStyle _style = new();

        [UsedImplicitly] public sealed class GizmosDrawer
        {
            [DrawGizmo(GizmoType.Selected)]
            static void OnSelected(NameTagGizmos t, GizmoType _) => t.DrawOnSelected(DrawGizmos);

            [DrawGizmo(GizmoType.NonSelected)]
            static void OnNonSelected(NameTagGizmos t, GizmoType _) => t.DrawOnNonSelected(DrawGizmos);

            static void DrawGizmos(NameTagGizmos target)
            {
                target._style.fontSize         = GetScaledFontSize(target.fontSize);
                target._style.normal.textColor = target.textColor;

                string tagName = string.IsNullOrWhiteSpace(target.nameOverride)
                    ? target.gameObject.name
                    : target.nameOverride;

                Transform t  = target.transform;
                Vector3   p  = t.position;
                Vector3   tp = p + VectorExtensions.GetNormal(target.axis) * GetScaledLength(target.distance);

                Handles.Label(tp, tagName, target._style);

                Gizmos.color = target.lineColor;
                Gizmos.DrawLine(p, tp);

                int GetScaledFontSize(int size)
                {
                    int result = size;
                    if (target.autoScaled)
                        result = (int)(result * GizmoUtils.ClampedInversedZoomLevel(target.fontSizeClippingDistance));
                    return result;
                }

                float GetScaledLength(float length)
                {
                    float result                           = length;
                    if (!target.distanceAutoScaled) result *= GizmoUtils.ZoomLevel;
                    return result;
                }
            }
        }
#endif
    }
}

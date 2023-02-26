#nullable enable


using JetBrains.Annotations;
using UnityEngine;
using Urun4m0r1.RekornTools.Math;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Urun4m0r1.RekornTools.Unity
{
    [DisallowMultipleComponent]
    public sealed class NameTagGizmos : MonoBehaviour, IGizmos
    {
#if UNITY_EDITOR
        [field: SerializeField] public  DrawMode DrawMode { get; set; } = DrawMode.OnSelected;
        [SerializeField]        private string   _nameOverride = string.Empty;

        [Header("Font Settings")]
        [SerializeField]
        private bool _autoScaled = true;
        [SerializeField]                  private Color _textColor                = Color.white;
        [SerializeField, Range(0f, 100f)] private int   _fontSize                 = 80;
        [SerializeField, Range(1f, 10f)]  private float _fontSizeClippingDistance = 5f;

        [Header("Position")]
        [SerializeField]
        private bool _distanceAutoScaled = true;
        [SerializeField] private Color                       _lineColor = Color.magenta;
        [SerializeField] private VectorExtensions.NormalAxis _axis      = VectorExtensions.NormalAxis.Up;

        [SerializeField, Range(0f, 10f)] private float _distance = 1.5f;

        private readonly GUIStyle _style = new();

        [UsedImplicitly] public sealed class GizmosDrawer
        {
            [DrawGizmo(GizmoType.Selected)] private static void OnSelected(NameTagGizmos t, GizmoType _) => t.DrawOnSelected(DrawGizmos);

            [DrawGizmo(GizmoType.NonSelected)] private static void OnNonSelected(NameTagGizmos t, GizmoType _) => t.DrawOnNonSelected(DrawGizmos);

            private static void DrawGizmos(NameTagGizmos target)
            {
                target._style.fontSize         = GetScaledFontSize(target._fontSize);
                target._style.normal.textColor = target._textColor;

                string tagName = string.IsNullOrWhiteSpace(target._nameOverride)
                    ? target.gameObject.name
                    : target._nameOverride;

                Transform t  = target.transform;
                Vector3   p  = t.position;
                Vector3   tp = p + VectorExtensions.GetNormal(target._axis) * GetScaledLength(target._distance);

                Handles.Label(tp, tagName, target._style);

                Gizmos.color = target._lineColor;
                Gizmos.DrawLine(p, tp);

                int GetScaledFontSize(int size)
                {
                    int result = size;
                    if (target._autoScaled)
                        result = (int)(result * GizmoUtils.ClampedInversedZoomLevel(target._fontSizeClippingDistance));
                    return result;
                }

                float GetScaledLength(float length)
                {
                    float result                           = length;
                    if (!target._distanceAutoScaled) result *= GizmoUtils.ZoomLevel;
                    return result;
                }
            }
        }
#endif
    }
}

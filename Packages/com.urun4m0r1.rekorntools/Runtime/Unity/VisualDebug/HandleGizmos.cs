#nullable enable

#if UNITY_EDITOR
using UnityEditor;
#endif

using JetBrains.Annotations;
using UnityEngine;

namespace Urun4m0r1.RekornTools.Unity
{
    [DisallowMultipleComponent]
    public sealed class HandleGizmos : MonoBehaviour, IGizmos
    {
#if UNITY_EDITOR
        [field: SerializeField] public DrawMode      DrawMode { get; set; } = DrawMode.OnSelected;
        [SerializeField]               TransformTool tool = TransformTool.Move | TransformTool.Rotate;

        bool _isSelected;

        [UsedImplicitly]
        [CanEditMultipleObjects]
        [CustomEditor(typeof(HandleGizmos))]
        public sealed class Drawer : Editor
        {
            void OnEnable()
            {
                SceneView.duringSceneGui -= OnScene;
                SceneView.duringSceneGui += OnScene;
            }

            void OnScene(SceneView sceneView)
            {
                var component = target as HandleGizmos;
                if (component == null) return;

                DrawGizmos(component);
            }

            [DrawGizmo(GizmoType.Selected)]
            static void OnSelected(HandleGizmos t, GizmoType _) => t._isSelected = true;

            [DrawGizmo(GizmoType.NonSelected)]
            static void OnNonSelected(HandleGizmos t, GizmoType _) => t._isSelected = false;

            static void DrawGizmos(HandleGizmos target)
            {
                if (target.tool == TransformTool.None) return;

                if (target._isSelected)
                {
                    UnityEditor.Tools.current = Tool.Custom;
                    if (target.WillDrawOnSelected()) DrawHandles(target);
                }
                else
                {
                    if (target.WillDrawOnNonSelected()) DrawHandles(target);
                }
            }

            static void DrawHandles(HandleGizmos target)
            {
                DrawCustomHandles(target, out Vector3 p, out Quaternion r, out Vector3 s);

                if (GUI.changed)
                {
                    Transform t = target.transform;

                    Undo.RecordObject(t, "Transform Change");
                    {
                        t.position      = p;
                        t.localRotation = r;
                        t.localScale    = s;
                    }
                }
            }

            static void DrawCustomHandles(HandleGizmos target, out Vector3 p, out Quaternion r, out Vector3 s)
            {
                Transform t = target.transform;
                p = t.position;
                r = t.localRotation;
                s = t.localScale;

                bool flagMove   = target.tool.HasFlag(TransformTool.Move);
                bool flagRotate = target.tool.HasFlag(TransformTool.Rotate);
                bool flagScale  = target.tool.HasFlag(TransformTool.Scale);

                if (flagMove && flagRotate && flagScale)
                {
                    Handles.TransformHandle(ref p, ref r, ref s);
                }
                else
                {
                    if (flagScale) s  = Handles.ScaleHandle(s, p, r, HandleUtility.GetHandleSize(p) * 1.25f);
                    if (flagMove) p   = Handles.PositionHandle(p, r);
                    if (flagRotate) r = Handles.RotationHandle(r, p);
                }
            }
        }
#endif
    }
}

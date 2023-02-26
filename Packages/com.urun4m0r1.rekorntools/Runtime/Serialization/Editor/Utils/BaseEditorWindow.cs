#nullable enable

using UnityEditor;
using UnityEngine;

namespace Urun4m0r1.RekornTools.Serialization.Editor
{
    public abstract class BaseEditorWindow<T> : EditorWindow
    {
        private static string PrefPath => $"{Application.identifier}/{typeof(T)}";

        private Vector2 _scrollPosition = Vector2.zero;
        private bool    _isSerializedFieldsReady;

        private void OnEnable()
        {
            EditorApplication.quitting += SaveSerializedData;
            LoadSerializedData();
            Enable();
            _isSerializedFieldsReady = true;
        }

        private void OnDisable()
        {
            EditorApplication.quitting -= SaveSerializedData;
            SaveSerializedData();
            Disable();
            _isSerializedFieldsReady = false;
        }

        private void LoadSerializedData()
        {
            var data = EditorPrefs.GetString(PrefPath, EditorJsonUtility.ToJson(this, false));
            EditorJsonUtility.FromJsonOverwrite(data, this);
        }

        private void SaveSerializedData()
        {
            var data = EditorJsonUtility.ToJson(this, false);
            EditorPrefs.SetString(PrefPath, data);
        }

        protected virtual void Enable()  { }
        protected virtual void Disable() { }

        public void OnGUI()
        {
            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition);
            {
                Undo.RecordObject(this, typeof(T).Name);
                {
                    if (_isSerializedFieldsReady) Draw();
                }
                Repaint();
            }
            GUILayout.EndScrollView();
        }

        protected abstract void Draw();
    }
}

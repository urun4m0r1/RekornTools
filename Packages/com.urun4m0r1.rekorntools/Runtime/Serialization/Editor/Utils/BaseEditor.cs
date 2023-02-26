#nullable enable

using UnityEditor;

namespace Urun4m0r1.RekornTools.Serialization.Editor
{
    public abstract class BaseEditor<T> : UnityEditor.Editor where T : UnityEngine.Object
    {
        private T _target;

        private void OnEnable() => _target = target as T;

        public override void OnInspectorGUI()
        {
            if (_target == null) return;

            Undo.RecordObject(_target, typeof(T).Name);
            {
                Draw(_target);
                if (_target is IValidate validate) validate.OnValidate();
            }
        }

        protected abstract void Draw(T t);
    }
}

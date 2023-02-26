#nullable enable

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
#endif

namespace Urun4m0r1.RekornTools.Unity
{
    [CreateAssetMenu(menuName = "ScriptableAction/Action")]
    public class ScriptableAction : ScriptableObject
    {
        [SerializeField] private bool willRaiseEvent = true;

        public virtual bool CanRaiseEvent { get; set; } = true;

        private readonly List<IEventListener> _listeners = new List<IEventListener>();
        public           void                 RegisterListener(IEventListener?   listener) => _listeners.Add(listener);
        public           void                 UnregisterListener(IEventListener? listener) => _listeners.Remove(listener);

        public void Invoke()
        {
            if (willRaiseEvent && CanRaiseEvent)
            {
                foreach (var listener in _listeners) listener.RaiseEvent();
            }
        }

        public override string ToString() => name;
    }

    public abstract class ScriptableAction<TValue> : ScriptableAction, IResetable
    {
        [SerializeField] private TValue initialInput;
        [SerializeField] private TValue runtimeInput;

        public TValue? InitialValue
        {
            get => initialInput;
            set
            {
                initialInput = value;

#if UNITY_EDITOR
                SyncValueInEditMode();
#endif
            }
        }

        public TValue? Value
        {
            get => runtimeInput;
            set
            {
                runtimeInput = value;
                Invoke();

#if UNITY_EDITOR
                SyncValueInEditMode();
#endif
            }
        }

        public void ResetValue()
        {
            if (runtimeInput.Equals(initialInput)) return;

            runtimeInput = initialInput;
            Invoke();
        }

        public static implicit operator TValue(ScriptableAction<TValue> scriptableEvent) => scriptableEvent.Value;

        public override string ToString() => $"{name} ({runtimeInput})";

#if UNITY_EDITOR
        private TValue _validatedValue;

        private void OnValidate()
        {
            SyncValueInEditMode();
            UpdateValueInPlayMode();
        }

        private void UpdateValueInPlayMode()
        {
            if (Application.isPlaying && !runtimeInput.Equals(_validatedValue))
            {
                _validatedValue = runtimeInput;
                Invoke();
            }
        }

        private void SyncValueInEditMode()
        {
            if (!Application.isPlaying) ResetValue();
        }

        private void OnEnable() => EditorApplication.playModeStateChanged += OnPlayModeStateChanged;

        private void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.EnteredEditMode) ResetValue();
        }
#endif
    }
}

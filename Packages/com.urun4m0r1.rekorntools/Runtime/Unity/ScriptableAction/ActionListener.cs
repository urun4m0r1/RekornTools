#nullable enable

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Urun4m0r1.RekornTools.Unity
{
    [Serializable]
    public sealed class ActionPairs : IEventListener
    {
        public string Invoker { get; set; }
        [SerializeField] private bool _showDebugLog = false;
        [SerializeField] private ScriptableAction _actionSender;
        [SerializeField] private UnityEvent _response;

        public void RegisterListener() => _actionSender.RegisterListener(this);
        public void UnregisterListener() => _actionSender.UnregisterListener(this);

        public void RaiseEvent()
        {
            LogAction();
            _response.Invoke();
        }

        private void LogAction()
        {
            if (_showDebugLog) Debug.Log($"{_actionSender}: Invoked {Invoker}");
        }
    }

    [Serializable]
    public sealed class ActionPairs<TValue> : IEventListener
    {
        public string Invoker { get; set; }
        [SerializeField] private bool _showDebugLog = false;
        [SerializeField] private ScriptableAction<TValue> _actionSender;
        [SerializeField] private UnityEvent<TValue> _response;

        public void RegisterListener() => _actionSender.RegisterListener(this);
        public void UnregisterListener() => _actionSender.UnregisterListener(this);

        public void RaiseEvent()
        {
            LogAction();
            _response.Invoke(_actionSender);
        }

        private void LogAction()
        {
            if (_showDebugLog) Debug.Log($"{_actionSender} ({typeof(TValue)}): Invoked {Invoker}");
        }
    }

    [ExecuteAlways]
    public class ActionListener : MonoBehaviour
    {
        [SerializeField] private List<ActionPairs> _actionsPairs = new List<ActionPairs>();

        private void Awake()
        {
            foreach (var actionPairs in _actionsPairs)
            {
                actionPairs.RegisterListener();
                actionPairs.Invoker = gameObject.name;
            }
        }

        private void OnDestroy()
        {
            foreach (var actionPairs in _actionsPairs) actionPairs.UnregisterListener();
        }
    }

    [ExecuteAlways]
    public abstract class ScriptableActionListener<TValue> : MonoBehaviour
    {
        [SerializeField] private List<ActionPairs<TValue>> _actionsPairs = new List<ActionPairs<TValue>>();

        private void Awake()
        {
            foreach (var actionPairs in _actionsPairs)
            {
                actionPairs.RegisterListener();
                actionPairs.Invoker = gameObject.name;
            }
        }

        private void OnDestroy()
        {
            foreach (var actionPairs in _actionsPairs) actionPairs.UnregisterListener();
        }
    }
}

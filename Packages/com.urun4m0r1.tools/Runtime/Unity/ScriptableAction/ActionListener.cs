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
        [SerializeField] private bool showDebugLog = false;
        [SerializeField] private ScriptableAction actionSender;
        [SerializeField] private UnityEvent response;

        public void RegisterListener() => actionSender.RegisterListener(this);
        public void UnregisterListener() => actionSender.UnregisterListener(this);

        public void RaiseEvent()
        {
            LogAction();
            response.Invoke();
        }

        private void LogAction()
        {
            if (showDebugLog) Debug.Log($"{actionSender}: Invoked {Invoker}");
        }
    }

    [Serializable]
    public sealed class ActionPairs<TValue> : IEventListener
    {
        public string Invoker { get; set; }
        [SerializeField] private bool showDebugLog = false;
        [SerializeField] private ScriptableAction<TValue> actionSender;
        [SerializeField] private UnityEvent<TValue> response;

        public void RegisterListener() => actionSender.RegisterListener(this);
        public void UnregisterListener() => actionSender.UnregisterListener(this);

        public void RaiseEvent()
        {
            LogAction();
            response.Invoke(actionSender);
        }

        private void LogAction()
        {
            if (showDebugLog) Debug.Log($"{actionSender} ({typeof(TValue)}): Invoked {Invoker}");
        }
    }

    [ExecuteAlways]
    public class ActionListener : MonoBehaviour
    {
        [SerializeField] private List<ActionPairs> actionsPairs = new List<ActionPairs>();

        private void Awake()
        {
            foreach (var actionPairs in actionsPairs)
            {
                actionPairs.RegisterListener();
                actionPairs.Invoker = gameObject.name;
            }
        }

        private void OnDestroy()
        {
            foreach (var actionPairs in actionsPairs) actionPairs.UnregisterListener();
        }
    }

    [ExecuteAlways]
    public abstract class ScriptableActionListener<TValue> : MonoBehaviour
    {
        [SerializeField] private List<ActionPairs<TValue>> actionsPairs = new List<ActionPairs<TValue>>();

        private void Awake()
        {
            foreach (var actionPairs in actionsPairs)
            {
                actionPairs.RegisterListener();
                actionPairs.Invoker = gameObject.name;
            }
        }

        private void OnDestroy()
        {
            foreach (var actionPairs in actionsPairs) actionPairs.UnregisterListener();
        }
    }
}

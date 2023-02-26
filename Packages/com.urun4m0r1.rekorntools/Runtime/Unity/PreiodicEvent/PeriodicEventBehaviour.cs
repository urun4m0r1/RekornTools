#nullable enable

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Urun4m0r1.RekornTools.Unity
{
    public sealed class PeriodicEventBehaviour : MonoBehaviour
    {
        // ReSharper disable MemberCanBePrivate.Global
        // ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
        // ReSharper disable UnusedAutoPropertyAccessor.Global

        [field: Header("Execution")]
        [field: SerializeField] public ReinvokeAction   ReinvokeAction { get; set; }         = ReinvokeAction.Ignore;
        [field: SerializeField] public Invoker          Invoker        { get; set; }         = Invoker.Manually;
        [field: SerializeField] public Interrupter      Interrupter    { get; set; }         = Interrupter.Manually;
        [field: SerializeField] public CoroutineLimiter Limiter        { get; private set; } = new();

        [field: Header("Time")]
        [field: SerializeField] public YieldDelay InitialDelay   { get; private set; } = new();
        [field: SerializeField] public YieldDelay ExecutionDelay { get; private set; } = new();
        [field: SerializeField] public YieldDelay PauseDelay     { get; private set; } = new();

        [field: Header("Status")]
        [field: SerializeField, ReadOnly] public bool  IsRunning     { get; private set; }
        [field: SerializeField, ReadOnly] public bool  IsExecuting   { get; private set; }
        [field: SerializeField, ReadOnly] public int   Execution     { get; private set; }
        [field: SerializeField, ReadOnly] public float ExecutionTime { get; private set; }

        [field: Header("Events")]
        [field: SerializeField] public UnityEvent OnInvoke          { get; private set; } = new();
        [field: SerializeField] public UnityEvent OnInterrupt       { get; private set; } = new();
        [field: SerializeField] public UnityEvent OnExecute         { get; private set; } = new();
        [field: SerializeField] public UnityEvent OnExecutionPaused { get; private set; } = new();

        // ReSharper restore MemberCanBePrivate.Global
        // ReSharper restore AutoPropertyCanBeMadeGetOnly.Local
        // ReSharper restore UnusedAutoPropertyAccessor.Global

        private Coroutine? _executeCoroutine;
        private bool       _isInvoked;

        [ContextMenu("Invoke")]
        public void Invoke()
        {
            if (Invoker.HasFlag(Invoker.Manually))
                InvokeInternal();
        }

        [ContextMenu("Interrupt")]
        public void Interrupt()
        {
            if (Interrupter.HasFlag(Interrupter.Manually))
                InterruptInternal();
        }

        private void Awake()
        {
            if (Invoker.HasFlag(Invoker.Awake))
                InvokeInternal();
        }

        private void Start()
        {
            if (Invoker.HasFlag(Invoker.Start))
                InvokeInternal();
        }

        private void OnEnable()
        {
            if (Invoker.HasFlag(Invoker.OnEnable))
                InvokeInternal();
        }

        private void OnDisable()
        {
            if (Interrupter.HasFlag(Interrupter.OnDisable))
                InterruptInternal();
        }

        private void OnApplicationQuit()
        {
            if (Interrupter.HasFlag(Interrupter.OnApplicationQuit))
                InterruptInternal();
        }

        private void OnDestroy()
        {
            if (Interrupter.HasFlag(Interrupter.OnDestroy))
                InterruptInternal();
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (hasFocus)
            {
                if (Invoker.HasFlag(Invoker.OnApplicationFocus))
                    InvokeInternal();
            }
            else
            {
                if (Interrupter.HasFlag(Interrupter.OnApplicationFocusLost))
                    InterruptInternal();
            }
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                if (Interrupter.HasFlag(Interrupter.OnApplicationPause))
                    InterruptInternal();
            }
            else
            {
                if (Invoker.HasFlag(Invoker.OnApplicationResume))
                    InvokeInternal();
            }
        }

        private void InvokeInternal()
        {
            if (_isInvoked)
                switch (ReinvokeAction)
                {
                    case ReinvokeAction.Ignore:
                        return;
                    case ReinvokeAction.Restart:
                        InterruptInternal();
                        break;
                    case ReinvokeAction.Stop:
                        InterruptInternal();
                        return;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

            _isInvoked = true;
            ResetExecutingStatus();

            _executeCoroutine = StartCoroutine(Execute());

            OnInvoke.Invoke();
        }

        private void InterruptInternal()
        {
            if (!_isInvoked) return;

            if (_executeCoroutine != null) StopCoroutine(_executeCoroutine);

            _isInvoked = false;
            ResetExecutingStatus();

            OnInterrupt.Invoke();
        }

        private void ResetExecutingStatus()
        {
            IsRunning     = false;
            IsExecuting   = false;
            Execution     = 0;
            ExecutionTime = 0;
        }

        private IEnumerator Execute()
        {
            yield return InitialDelay.WaitForDelay;

            IsRunning = true;

            do
            {
                Execution++;
                float startupTime = Time.realtimeSinceStartup;

                StartExecute();
                yield return ExecutionDelay.WaitForDelay;

                PauseExecution();
                yield return PauseDelay.WaitForDelay;

                ExecutionTime += Time.realtimeSinceStartup - startupTime;
                IsRunning = !Limiter.IsLimitReached(Execution, ExecutionTime);
            } while (IsRunning);

            _isInvoked = false;
            ResetExecutingStatus();

            void StartExecute()
            {
                IsExecuting = true;
                OnExecute.Invoke();
            }

            void PauseExecution()
            {
                IsExecuting = false;
                OnExecutionPaused.Invoke();
            }
        }
    }
}

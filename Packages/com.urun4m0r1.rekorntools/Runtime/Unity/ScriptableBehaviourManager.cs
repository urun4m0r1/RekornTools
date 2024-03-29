﻿#nullable enable

using System.Collections.Generic;
using UnityEngine;

namespace Urun4m0r1.RekornTools.Unity
{
    public class ScriptableBehaviourManager : MonoBehaviour
    {
        [SerializeField] private List<ScriptableBehaviour> _behaviours = new();

        protected virtual void Awake()
        {
            foreach (var behaviour in _behaviours) behaviour.Awake();
        }

        protected virtual void Start()
        {
            foreach (var behaviour in _behaviours) behaviour.Start();
        }

        protected virtual void OnEnable()
        {
            foreach (var behaviour in _behaviours) behaviour.OnEnable();
        }

        protected virtual void OnDisable()
        {
            foreach (var behaviour in _behaviours) behaviour.OnDisable();
        }

        protected virtual void OnDestroy()
        {
            foreach (var behaviour in _behaviours) behaviour.OnDestroy();
        }

        protected virtual void OnApplicationQuit()
        {
            foreach (var behaviour in _behaviours) behaviour.OnApplicationQuit();
        }

        protected virtual void OnApplicationFocus(bool focus)
        {
            foreach (var behaviour in _behaviours) behaviour.OnApplicationFocus(focus);
        }

        protected virtual void OnApplicationPause(bool pause)
        {
            foreach (var behaviour in _behaviours) behaviour.OnApplicationPause(pause);
        }

        protected virtual void Update()
        {
            foreach (var behaviour in _behaviours) behaviour.Update();
        }

        protected virtual void FixedUpdate()
        {
            foreach (var behaviour in _behaviours) behaviour.FixedUpdate();
        }

        protected virtual void LateUpdate()
        {
            foreach (var behaviour in _behaviours) behaviour.LateUpdate();
        }
    }
}

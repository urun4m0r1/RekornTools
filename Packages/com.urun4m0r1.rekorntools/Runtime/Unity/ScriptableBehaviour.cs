using UnityEngine;

namespace Urun4m0r1.RekornTools.Unity
{
    public class ScriptableBehaviour : ScriptableObject
    {
        public virtual void Awake() { }
        public virtual void Start() { }
        public virtual void OnEnable() { }
        public virtual void OnDisable() { }
        public virtual void OnDestroy() { }
        public virtual void OnApplicationQuit() { }
        public virtual void OnApplicationFocus(bool focus) { }
        public virtual void OnApplicationPause(bool pause) { }
        public virtual void Update() { }
        public virtual void FixedUpdate() { }
        public virtual void LateUpdate() { }
    }

    [CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
    public class Item : ScriptableBehaviour
    {
        public string itemName;
    }
}

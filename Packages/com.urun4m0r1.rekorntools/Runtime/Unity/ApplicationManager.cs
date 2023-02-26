using UnityEngine;
using UnityEngine.Events;

namespace Urun4m0r1.RekornTools.Unity
{
    public class ApplicationManager : MonoBehaviour
    {
        [SerializeField] private UnityEvent onApplicationStart;

        private void Awake()
        {
            SetTargetFrameRate(10);
            onApplicationStart.Invoke();
        }

        public void SetTargetFrameRate(IntAction targetFrameRate)
        {
            SetTargetFrameRate(targetFrameRate.Value);
        }

        public void SetTargetFrameRate(int targetFrameRate)
        {
            Application.targetFrameRate = targetFrameRate;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape)) Quit();
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}

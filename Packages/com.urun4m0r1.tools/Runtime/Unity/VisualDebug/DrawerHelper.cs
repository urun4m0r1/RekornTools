#if UNITY_EDITOR
#nullable enable

using System;
using UnityEngine;

namespace Urun4m0r1.RekornTools.Unity
{
    public static class DrawerHelper
    {
        public static bool WillDrawOnSelected(this IGizmos target) =>
            target.DrawMode is DrawMode.Always or DrawMode.OnSelected;
        public static bool WillDrawOnNonSelected(this IGizmos target) =>
            target.DrawMode is DrawMode.Always or DrawMode.OnNonSelected;

        public static void DrawOnSelected<T>(this T target, Action<T> action)
            where T : MonoBehaviour, IGizmos
        {
            if (target.WillDrawOnSelected()) action(target);
        }

        public static void DrawOnNonSelected<T>(this T target, Action<T> action)
            where T : MonoBehaviour, IGizmos
        {
            if (target.WillDrawOnNonSelected()) action(target);
        }
    }
}
#endif

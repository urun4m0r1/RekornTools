#nullable enable

using UnityEngine;
using Urun4m0r1.RekornTools.DesignPatterns;

namespace Urun4m0r1.RekornTools.Tests
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Rekorn Tools (Test)/Mono Singleton/(Test) Mono Singleton Tester")]
    public sealed class MonoSingletonTester : MonoSingleton<MonoSingletonTester> { }
}

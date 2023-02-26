#nullable enable

using Rekorn.Tools.DesignPatterns;
using UnityEngine;

namespace Rekorn.Tools.Tests
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Rekorn Tools (Test)/Mono Singleton/(Test) Mono Singleton Tester")]
    public sealed class MonoSingletonTester : MonoSingleton<MonoSingletonTester> { }
}

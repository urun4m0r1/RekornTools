#nullable enable

namespace Rekorn.Tools.DesignPatterns
{
    /// <summary>
    /// <b>Use this code to initialize singleton instance on application start.</b>
    /// <code>
    /// [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    /// private static void OnApplicationInitialized() =&gt; TryCreateInstance();
    /// </code>
    /// </summary>
    public interface ISingleton
    {
        public bool IsSingleton { get; }
    }

    /// <inheritdoc />
    /// <summary>
    /// <b>Use this code to initialize singleton instance before any scene loaded.</b>
    /// <code>
    /// [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    /// private static void OnEngineInitialized() =&gt; TryCreateInstance();
    /// </code>
    /// </summary>
    public interface IMonoSingleton : ISingleton { }
}

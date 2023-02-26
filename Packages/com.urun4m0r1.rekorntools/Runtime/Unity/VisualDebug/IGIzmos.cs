#nullable enable

namespace Urun4m0r1.RekornTools.Unity
{
    public interface IGizmos
    {
#if UNITY_EDITOR
        DrawMode DrawMode { get; set; }
#endif
    }
}

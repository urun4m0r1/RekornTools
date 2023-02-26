namespace Rekorn.Tools.Unity
{
    public interface IGizmos
    {
#if UNITY_EDITOR
        DrawMode DrawMode { get; set; }
#endif
    }
}

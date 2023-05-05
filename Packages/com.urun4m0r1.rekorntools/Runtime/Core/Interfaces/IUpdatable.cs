#nullable enable

namespace Urun4m0r1.RekornTools
{
    public interface IUpdatable
    {
        bool IsUpdateEnabled { get; }
        void OnUpdate(float deltaTime);
    }

    public interface IFixedUpdatable
    {
        bool IsFixedUpdateEnabled { get; }
        void OnFixedUpdate(float deltaTime);
    }

    public interface ILateUpdatable
    {
        bool IsLateUpdateEnabled { get; }
        void OnLateUpdate(float deltaTime);
    }
}

#nullable enable

using JetBrains.Annotations;
using Rekorn.Tools.DesignPatterns;

namespace Rekorn.Tools.Tests
{
    [UsedImplicitly]
    public class SingletonTester : Singleton<SingletonTester>
    {
        private SingletonTester() { }
    }

#region Single_Constructor_Parameterless
    [UsedImplicitly]
    public class Public_SingletonTester : Singleton<Public_SingletonTester>
    {
        // ReSharper disable once EmptyConstructor
        public Public_SingletonTester() { }
    }

    [UsedImplicitly]
    public class Protected_SingletonTester : Singleton<Protected_SingletonTester>
    {
        protected Protected_SingletonTester() { }
    }

    [UsedImplicitly]
    public class Private_SingletonTester : Singleton<Private_SingletonTester>
    {
        private Private_SingletonTester() { }
    }
#endregion // Single_Constructor_Parameterless

#region Single_Constructor_WithParameters
    [UsedImplicitly]
    public class Public_WithParameter_SingletonTester : Singleton<Public_WithParameter_SingletonTester>
    {
        public Public_WithParameter_SingletonTester(int a) { }
    }

    [UsedImplicitly]
    public class Protected_WithParameter_SingletonTester : Singleton<Protected_WithParameter_SingletonTester>
    {
        protected Protected_WithParameter_SingletonTester(int a) { }
    }

    [UsedImplicitly]
    public class Private_WithParameter_SingletonTester : Singleton<Private_WithParameter_SingletonTester>
    {
        private Private_WithParameter_SingletonTester(int a) { }
    }
#endregion // Single_Constructor_WithParameters

#region Multiple_Constructors
    [UsedImplicitly]
    public class Multiple_Public_SingletonTester : Singleton<Multiple_Public_SingletonTester>
    {
        public Multiple_Public_SingletonTester() { }

        public Multiple_Public_SingletonTester(int a) { }
    }

    [UsedImplicitly]
    public class Multiple_Protected_SingletonTester : Singleton<Multiple_Protected_SingletonTester>
    {
        protected Multiple_Protected_SingletonTester() { }

        protected Multiple_Protected_SingletonTester(int a) { }
    }

    [UsedImplicitly]
    public class Multiple_Private_SingletonTester : Singleton<Multiple_Private_SingletonTester>
    {
        private Multiple_Private_SingletonTester() { }

        private Multiple_Private_SingletonTester(int a) { }
    }
#endregion // Multiple_Constructors
}

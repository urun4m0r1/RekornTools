#nullable enable

using System;
using NUnit.Framework;

namespace Urun4m0r1.RekornTools.Tests.EditMode
{
    public sealed class MonoSingletonTests
    {
        [SetUp]
        public void SetUp()
        {
            MonoSingletonTester.SetupForTests();
        }

        [TearDown]
        public void TearDown()
        {
            MonoSingletonTester.TearDownForTests();
        }

        [Test]
        public void CreateInstance_Passes()
        {
            Assert.DoesNotThrow(MonoSingletonTester.CreateInstance);
        }

        [Test]
        public void CreateInstance_Multiple_Times_Throws()
        {
            Assert.DoesNotThrow(MonoSingletonTester.CreateInstance);
            Assert.Throws<InvalidOperationException>(MonoSingletonTester.CreateInstance);
        }

        [Test]
        public void TryCreateInstance_Will_Return_True()
        {
            Assert.IsTrue(MonoSingletonTester.TryCreateInstance());
        }

        [Test]
        public void TryCreateInstance_Multiple_Times_Will_Return_False()
        {
            Assert.IsTrue(MonoSingletonTester.TryCreateInstance());
            Assert.IsFalse(MonoSingletonTester.TryCreateInstance());
        }

        [Test]
        public void HasInstance_Will_Return_True()
        {
            MonoSingletonTester.CreateInstance();
            Assert.IsTrue(MonoSingletonTester.HasInstance);
        }

        [Test]
        public void HasInstance_Will_Return_False()
        {
            Assert.IsFalse(MonoSingletonTester.HasInstance);
        }

        [Test]
        public void InstanceOrNull_Will_Return_Instance()
        {
            MonoSingletonTester.CreateInstance();
            Assert.IsNotNull(MonoSingletonTester.InstanceOrNull!);
        }

        [Test]
        public void InstanceOrNull_Will_Return_Null()
        {
            Assert.IsNull(MonoSingletonTester.InstanceOrNull!);
        }

        [Test]
        public void InstanceOrNull_Will_Not_Create_Instance()
        {
            Assert.IsNull(MonoSingletonTester.InstanceOrNull!);
            Assert.IsFalse(MonoSingletonTester.HasInstance);
        }

        [Test]
        public void Instance_Will_Return_Instance()
        {
            Assert.IsNotNull(MonoSingletonTester.Instance);
        }

        [Test]
        public void InstanceOrNull_Equals_Instance()
        {
            MonoSingletonTester.CreateInstance();
            Assert.AreEqual(MonoSingletonTester.InstanceOrNull!, MonoSingletonTester.Instance);
        }
    }
}

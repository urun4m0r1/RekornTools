#nullable enable

using System;
using NUnit.Framework;

namespace Rekorn.Tools.Tests.EditMode
{
    public sealed class SingletonTests
    {
        [SetUp]
        public void SetUp()
        {
            SingletonTester.SetupForTests();
        }

        [TearDown]
        public void TearDown()
        {
            SingletonTester.TearDownForTests();
        }

        [Test]
        public void CreateInstance_Passes()
        {
            Assert.DoesNotThrow(SingletonTester.CreateInstance);
        }

        [Test]
        public void CreateInstance_Multiple_Times_Throws()
        {
            Assert.DoesNotThrow(SingletonTester.CreateInstance);
            Assert.Throws<InvalidOperationException>(SingletonTester.CreateInstance);
        }

        [Test]
        public void TryCreateInstance_Will_Return_True()
        {
            Assert.IsTrue(SingletonTester.TryCreateInstance());
        }

        [Test]
        public void TryCreateInstance_Multiple_Times_Will_Return_False()
        {
            Assert.IsTrue(SingletonTester.TryCreateInstance());
            Assert.IsFalse(SingletonTester.TryCreateInstance());
        }

        [Test]
        public void HasInstance_Will_Return_True()
        {
            SingletonTester.CreateInstance();
            Assert.IsTrue(SingletonTester.HasInstance);
        }

        [Test]
        public void HasInstance_Will_Return_False()
        {
            Assert.IsFalse(SingletonTester.HasInstance);
        }

        [Test]
        public void InstanceOrNull_Will_Return_Instance()
        {
            SingletonTester.CreateInstance();
            Assert.IsNotNull(SingletonTester.InstanceOrNull!);
        }

        [Test]
        public void InstanceOrNull_Will_Return_Null()
        {
            Assert.IsNull(SingletonTester.InstanceOrNull!);
        }

        [Test]
        public void InstanceOrNull_Will_Not_Create_Instance()
        {
            Assert.IsNull(SingletonTester.InstanceOrNull!);
            Assert.IsFalse(SingletonTester.HasInstance);
        }

        [Test]
        public void Instance_Will_Return_Instance()
        {
            Assert.IsNotNull(SingletonTester.Instance);
        }

        [Test]
        public void InstanceOrNull_Equals_Instance()
        {
            SingletonTester.CreateInstance();
            Assert.AreEqual(SingletonTester.InstanceOrNull!, SingletonTester.Instance);
        }
    }

    public sealed class SingletonTests_Constructor_Validation
    {
        [Test]
        public void CreateInstance_With_NonPublic_Constructor_Passes()
        {
            Protected_SingletonTester.SetupForTests();
            Private_SingletonTester.SetupForTests();
            {
                Assert.DoesNotThrow(Protected_SingletonTester.CreateInstance);
                Assert.DoesNotThrow(Private_SingletonTester.CreateInstance);
            }
            Protected_SingletonTester.TearDownForTests();
            Private_SingletonTester.TearDownForTests();
        }

        [Test]
        public void CreateInstance_With_Public_Constructor_Throws()
        {
            Public_SingletonTester.SetupForTests();
            {
                Assert.Throws<NotSupportedException>(Public_SingletonTester.CreateInstance);
            }
            Public_SingletonTester.TearDownForTests();
        }

        [Test]
        public void CreateInstance_With_Parameter_Throws()
        {
            Public_WithParameter_SingletonTester.SetupForTests();
            Protected_WithParameter_SingletonTester.SetupForTests();
            Private_WithParameter_SingletonTester.SetupForTests();
            {
                Assert.Throws<NotSupportedException>(Public_WithParameter_SingletonTester.CreateInstance);
                Assert.Throws<NotSupportedException>(Protected_WithParameter_SingletonTester.CreateInstance);
                Assert.Throws<NotSupportedException>(Private_WithParameter_SingletonTester.CreateInstance);
            }
            Public_WithParameter_SingletonTester.TearDownForTests();
            Protected_WithParameter_SingletonTester.TearDownForTests();
            Private_WithParameter_SingletonTester.TearDownForTests();
        }

        [Test]
        public void CreateInstance_With_Multiple_Constructors_Throws()
        {
            Multiple_Public_SingletonTester.SetupForTests();
            Multiple_Protected_SingletonTester.SetupForTests();
            Multiple_Private_SingletonTester.SetupForTests();
            {
                Assert.Throws<NotSupportedException>(Multiple_Public_SingletonTester.CreateInstance);
                Assert.Throws<NotSupportedException>(Multiple_Protected_SingletonTester.CreateInstance);
                Assert.Throws<NotSupportedException>(Multiple_Private_SingletonTester.CreateInstance);
            }
            Multiple_Public_SingletonTester.TearDownForTests();
            Multiple_Protected_SingletonTester.TearDownForTests();
            Multiple_Private_SingletonTester.TearDownForTests();
        }
    }
}

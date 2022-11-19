#nullable enable

using NUnit.Framework;
using Rekorn.Tools.Utils;

namespace Rekorn.Tools.Tests.EditMode
{
    public sealed class ObjectBoxerTests
    {
        private sealed class TestClass
        {
            public int Value { get; set; }
        }

        private struct TestStruct
        {
            public int Value { get; set; }
        }

        [Test]
        public void Test_Class_ObjectBoxer_With_Empty_Parameter()
        {
            var boxer = new ObjectBoxer<TestClass>();

            var castedObject  = (TestClass)boxer.Box;
            var unboxedObject = boxer.Unbox();

            castedObject.Value = 1;

            Assert.AreEqual(1, castedObject.Value);
            Assert.AreEqual(1, unboxedObject.Value);

            Assert.AreSame(castedObject, unboxedObject);
        }

        [Test]
        public void Test_Class_ObjectBoxer_With_Existing_Object()
        {
            var targetObject = new TestClass();

            var boxer = new ObjectBoxer<TestClass>(targetObject);

            var castedObject  = (TestClass)boxer.Box;
            var unboxedObject = boxer.Unbox();

            castedObject.Value = 1;

            Assert.AreEqual(1, targetObject.Value);
            Assert.AreEqual(1, castedObject.Value);
            Assert.AreEqual(1, unboxedObject.Value);

            Assert.AreSame(targetObject, castedObject);
            Assert.AreSame(targetObject, unboxedObject);
            Assert.AreSame(castedObject, unboxedObject);
        }

        [Test]
        public void Test_Struct_ObjectBoxer_With_Empty_Parameter()
        {
            var boxer = new ObjectBoxer<TestStruct>();

            var castedObject  = (TestStruct)boxer.Box;
            var unboxedObject = boxer.Unbox();

            castedObject.Value = 1;

            Assert.AreEqual(1, castedObject.Value);
            Assert.AreEqual(0, unboxedObject.Value);

            Assert.AreNotSame(castedObject, unboxedObject);
        }

        [Test]
        public void Test_Struct_ObjectBoxer_With_Existing_Object()
        {
            var targetObject = new TestStruct();

            var boxer = new ObjectBoxer<TestStruct>(targetObject);

            var castedObject  = (TestStruct)boxer.Box;
            var unboxedObject = boxer.Unbox();

            castedObject.Value = 1;

            Assert.AreEqual(0, targetObject.Value);
            Assert.AreEqual(1, castedObject.Value);
            Assert.AreEqual(0, unboxedObject.Value);

            Assert.AreNotSame(targetObject, castedObject);
            Assert.AreNotSame(targetObject, unboxedObject);
            Assert.AreNotSame(castedObject, unboxedObject);
        }
    }
}

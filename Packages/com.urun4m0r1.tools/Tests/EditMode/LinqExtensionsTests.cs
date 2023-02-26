#nullable enable

using System.Linq;
using NUnit.Framework;
using Rekorn.Tools.Utils;

namespace Rekorn.Tools.Tests.EditMode
{
    public sealed class LinqExtensionsTests
    {
        private static readonly string John   = "John";
        private static readonly string Mary   = "Mary";
        private static readonly int    Seven  = 7;
        private static readonly int    Eleven = 11;

        private sealed class TestClass
        {
            public string Name { get; }
            public int?   Age  { get; }

            public TestClass(string name, int age)
            {
                Name = name;
                Age  = age;
            }

            public TestClass()
            {
                Name = John;
                Age  = 7;
            }
        }

        [Test]
        public void Test_CreateNewItems_Can_Create_Instances()
        {
            var list = LinqExtensions.CreateNewItems<TestClass>(10).ToList();

            Assert.AreEqual(10, list.Count);

            foreach (var item in list)
            {
                Assert.IsNotNull(item);
                Assert.AreEqual(John,  item.Name);
                Assert.AreEqual(Seven, item.Age!);
            }
        }

        [Test]
        public void Test_CreateDefaultItems_Can_Create_Instances()
        {
            var list = LinqExtensions.CreateDefaultItems<TestClass>(10).ToList();

            Assert.AreEqual(10, list.Count);

            foreach (var item in list)
            {
                Assert.IsNull(item!);
            }
        }

        [Test]
        public void Test_CreateItems_Can_Create_Instances_With_Generator()
        {
            var list = LinqExtensions.CreateItems(10, static () => new TestClass(Mary, Eleven)).ToList();

            Assert.AreEqual(10, list.Count);

            foreach (var item in list)
            {
                Assert.IsNotNull(item!);
                Assert.AreEqual(Mary,   item?.Name!);
                Assert.AreEqual(Eleven, item?.Age!);
            }
        }

        [Test]
        public void Test_CreateItems_Can_Create_Null_Instances_With_Generator()
        {
            var list = LinqExtensions.CreateItems<TestClass>(10, static () => null).ToList();

            Assert.AreEqual(10, list.Count);

            foreach (var item in list)
            {
                Assert.IsNull(item!);
            }
        }
    }
}

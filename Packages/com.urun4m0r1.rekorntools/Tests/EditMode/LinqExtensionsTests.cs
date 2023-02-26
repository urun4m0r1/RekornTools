#nullable enable

using System.Linq;
using NUnit.Framework;
using Urun4m0r1.RekornTools.Utils;

namespace Urun4m0r1.RekornTools.Tests.EditMode
{
    public sealed class LinqExtensionsTests
    {
        private static readonly string s_john   = "John";
        private static readonly string s_mary   = "Mary";
        private static readonly int    s_seven  = 7;
        private static readonly int    s_eleven = 11;

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
                Name = s_john;
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
                Assert.AreEqual(s_john,  item.Name);
                Assert.AreEqual(s_seven, item.Age!);
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
            var list = LinqExtensions.CreateItems(10, static () => new TestClass(s_mary, s_eleven)).ToList();

            Assert.AreEqual(10, list.Count);

            foreach (var item in list)
            {
                Assert.IsNotNull(item!);
                Assert.AreEqual(s_mary,   item?.Name!);
                Assert.AreEqual(s_eleven, item?.Age!);
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

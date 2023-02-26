#nullable enable

using System;
using NUnit.Framework;
using Urun4m0r1.RekornTools.Utils;

namespace Urun4m0r1.RekornTools.Tests.EditMode
{
    public class CacheTests
    {
        private readonly Func<int, string> _generator = static i => i.ToString();

        [Test]
        public void Test_Can_Generate_And_Cache_Non_Cached_Item()
        {
            var cache = new Cache<int, string>(_generator);

            Assert.IsFalse(cache.IsCached(1));

            Assert.AreEqual("1", cache.Request(1)!);

            Assert.IsTrue(cache.IsCached(1));
        }

        [Test]
        public void Test_Can_Add_Cache()
        {
            var cache = new Cache<int, string>(_generator);

            Assert.IsFalse(cache.IsCached(1));

            cache.Add(1, "1");

            Assert.IsTrue(cache.IsCached(1));

            Assert.AreEqual("1", cache.Request(1)!);
        }

        [Test]
        public void Test_Can_Remove_Cache()
        {
            var cache = new Cache<int, string>(_generator);

            cache.Add(1, "1");

            cache.Remove(1);

            Assert.IsFalse(cache.IsCached(1));
        }

        [Test]
        public void Test_Can_Clear_All_Cache()
        {
            var cache = new Cache<int, string>(_generator);

            cache.Add(1, "1");
            cache.Add(2, "2");
            cache.Add(3, "3");

            cache.Clear();

            Assert.IsFalse(cache.IsCached(1));
            Assert.IsFalse(cache.IsCached(2));
            Assert.IsFalse(cache.IsCached(3));
        }
    }
}

#nullable enable

using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Urun4m0r1.RekornTools.Tests.PlayMode
{
    public sealed class TestTemplate
    {
        [SetUp]
        public void SetUp()
        {
            Assert.Pass();
        }

        [TearDown]
        public void TearDown()
        {
            Assert.Pass();
        }

        [Test]
        public void Test_Passes()
        {
            Assert.Pass();
        }

        [UnityTest]
        public IEnumerator UnityTest_Passes()
        {
            yield return null;
            Assert.Pass();
        }
    }
}

#nullable enable

using System.Collections.Generic;
using NUnit.Framework;
using Urun4m0r1.RekornTools.Utils;

namespace Urun4m0r1.RekornTools.Tests.EditMode
{
    public sealed class DictionaryExtensionsTests
    {
        private enum DictKeys
        {
            A,
            B,
            C,
        }

        private sealed class DictValue
        {
            public int Value { get; set; }

            public DictValue()
            {
                Value = -1;
            }

            public DictValue(int value)
            {
                Value = value;
            }
        }

        private readonly Dictionary<DictKeys, int> _structValuesDictionary = new();

        private readonly Dictionary<DictKeys, DictValue> _classValuesDictionary = new();

        private readonly Dictionary<DictKeys, DictValue?> _nullableClassValuesDictionary = new();

        [SetUp]
        public void SetUp()
        {
            _structValuesDictionary.Clear();
            _structValuesDictionary.Add(DictKeys.A, 1);
            _structValuesDictionary.Add(DictKeys.B, 2);

            _classValuesDictionary.Clear();
            _classValuesDictionary.Add(DictKeys.A, new DictValue(1));
            _classValuesDictionary.Add(DictKeys.B, new DictValue(2));

            _nullableClassValuesDictionary.Clear();
            _nullableClassValuesDictionary.Add(DictKeys.A, new DictValue(1));
            _nullableClassValuesDictionary.Add(DictKeys.B, new DictValue(2));
        }

        [TearDown]
        public void TearDown()
        {
            _structValuesDictionary.Clear();
            _classValuesDictionary.Clear();
            _nullableClassValuesDictionary.Clear();
        }

        private void AssertStructValuesDictionary(int expected)
        {
            Assert.AreEqual(1, _structValuesDictionary[DictKeys.A]);
            Assert.AreEqual(2, _structValuesDictionary[DictKeys.B]);

            Assert.IsTrue(_structValuesDictionary.ContainsKey(DictKeys.C));
            Assert.AreEqual(expected, _structValuesDictionary[DictKeys.C]);
        }

        private void AssertClassValuesDictionary(int expected)
        {
            Assert.AreEqual(1, _classValuesDictionary[DictKeys.A]!.Value);
            Assert.AreEqual(2, _classValuesDictionary[DictKeys.B]!.Value);

            Assert.IsTrue(_classValuesDictionary.ContainsKey(DictKeys.C));
            Assert.AreEqual(expected, _classValuesDictionary[DictKeys.C]!.Value);
        }

        private void AssertNullableClassValuesDictionary(int? expected)
        {
            Assert.AreEqual(1, _nullableClassValuesDictionary[DictKeys.A]?.Value!);
            Assert.AreEqual(2, _nullableClassValuesDictionary[DictKeys.B]?.Value!);

            Assert.IsTrue(_nullableClassValuesDictionary.ContainsKey(DictKeys.C));
            Assert.AreEqual(expected!, _nullableClassValuesDictionary[DictKeys.C]?.Value!);
        }

        [Test]
        public void Update_Keys_With_Default_Or_Null_Values_Will_Update_Dictionary()
        {
            _structValuesDictionary.UpdateKeysWithDefaultValues();
            AssertStructValuesDictionary(default);

            _nullableClassValuesDictionary.UpdateKeysWithNullValues();
            AssertNullableClassValuesDictionary(null);
        }

        [Test]
        public void Update_Keys_With_New_Values_Will_Update_Dictionary()
        {
            _structValuesDictionary.UpdateKeysWithNewValues();
            AssertStructValuesDictionary(new int());

            _classValuesDictionary.UpdateKeysWithNewValues();
            AssertClassValuesDictionary(new DictValue().Value);

            _nullableClassValuesDictionary.UpdateKeysWithNewValues();
            AssertNullableClassValuesDictionary(new DictValue().Value);
        }

        [Test]
        public void Update_Keys_With_Value_Factory_Will_Update_Dictionary()
        {
            _structValuesDictionary.UpdateKeysWithValueFactory(static _ => 3);
            AssertStructValuesDictionary(3);

            _classValuesDictionary.UpdateKeysWithValueFactory(static _ => new DictValue(3));
            AssertClassValuesDictionary(3);

            _nullableClassValuesDictionary.UpdateKeysWithValueFactory(static _ => new DictValue(3));
            AssertNullableClassValuesDictionary(3);
        }
    }
}

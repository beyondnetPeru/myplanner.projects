using BeyondNet.Ddd.Test.Stubs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeyondNet.Ddd.Test
{
    [TestClass]
    public class EnumerationTest
    {
        [TestMethod]
        public void GetAll_ShouldReturnAllValues()
        {
            var values = Enumeration.GetAll<MockEnumeration>().ToList();
            Assert.AreEqual(2, values.Count);
            Assert.AreEqual(MockEnumeration.First, values[0]);
            Assert.AreEqual(MockEnumeration.Second, values[1]);
        }

        [TestMethod]
        public void FromValue_ShouldReturnCorrectValue()
        {
            var value = Enumeration.FromValue<MockEnumeration>(1);
            Assert.AreEqual(MockEnumeration.First, value);
        }

        [TestMethod]
        public void FromDisplayName_ShouldReturnCorrectValue()
        {
            var value = Enumeration.FromDisplayName<MockEnumeration>("First");
            Assert.AreEqual(MockEnumeration.First, value);
        }

        [TestMethod]
        public void AbsoluteDifference_ShouldReturnCorrectValue()
        {
            var value = Enumeration.AbsoluteDifference(MockEnumeration.First, MockEnumeration.Second);
            Assert.AreEqual(1, value);
        }

        [TestMethod]
        public void Equals_ShouldReturnTrue()
        {
            var value = MockEnumeration.First.Equals(MockEnumeration.First);
            Assert.IsTrue(value);
        }

        [TestMethod]
        public void Equals_ShouldReturnFalse()
        {
            var value = MockEnumeration.First.Equals(MockEnumeration.Second);
            Assert.IsFalse(value);
        }

        [TestMethod]
        public void GetHashCode_ShouldReturnCorrectValue()
        {
            var value = MockEnumeration.First.GetHashCode();
            Assert.AreEqual(1, value);
        }

        [TestMethod]
        public void ToString_ShouldReturnCorrectValue()
        {
            var value = MockEnumeration.First.ToString();
            Assert.AreEqual("First", value);
        }

        [TestMethod]
        public void CompareTo_ShouldReturnCorrectValue()
        {
            var value = MockEnumeration.First.CompareTo(MockEnumeration.Second);
            Assert.AreEqual(-1, value);
        }

        [TestMethod]
        public void CompareTo_ShouldReturnZero()
        {
            var value = MockEnumeration.First.CompareTo(MockEnumeration.First);
            Assert.AreEqual(0, value);
        }

        [TestMethod]
        public void CompareTo_ShouldReturnOne()
        {
            var value = MockEnumeration.Second.CompareTo(MockEnumeration.First);
            Assert.AreEqual(1, value);
        }

        [TestMethod]
        public void Parse_ShouldThrowException()
        {
            Assert.ThrowsException<InvalidOperationException>(() => Enumeration.FromValue<MockEnumeration>(3));
        }

        [TestMethod]
        public void Parse_ShouldThrowExceptionWithCorrectMessage()
        {
            try
            {
                Enumeration.FromValue<MockEnumeration>(3);
            }
            catch (InvalidOperationException ex)
            {
                Assert.AreEqual("'3' is not a valid value in BeyondNet.Ddd.Test.Stubs.MockEnumeration", ex.Message);
            }
        }

        [TestMethod]
        public void Parse_ShouldThrowExceptionWithCorrectMessage2()
        {
            try
            {
                Enumeration.FromDisplayName<MockEnumeration>("Third");
            }
            catch (InvalidOperationException ex)
            {
                Assert.AreEqual("'Third' is not a valid display name in BeyondNet.Ddd.Test.Stubs.MockEnumeration", ex.Message);
            }
        }

    }
}

using BeyondNet.Ddd.Rules.Impl;
using BeyondNet.Ddd.Test.Stubs;

namespace BeyondNet.Ddd.Test
{
    [TestClass]
    public class ValueObjectTest
    {
        [TestMethod]
        public void Should_IsDirty_Be_False()
        {
            var fieldName = FieldName.Create("foo");

            Assert.IsTrue(fieldName.IsNew);
            Assert.IsFalse(fieldName.IsDirty);
        }

        [TestMethod]
        public void Should_IsDirty_Be_True()
        {
            var fieldName = FieldName.Create("foo");

            fieldName.Value = "bar";

            Assert.IsTrue(fieldName.IsDirty);
            Assert.IsFalse(fieldName.IsNew);
        }


        [TestMethod]
        public void Should_Implement_ValueObject()
        {
            var fieldName = FieldName.Create("foo");

            Assert.IsInstanceOfType(fieldName, typeof(ValueObject<string>));
        }

        [TestMethod]
        public void Should_Be_Equal()
        {
            var fieldName1 = FieldName.Create("foo");
            var fieldName2 = FieldName.Create("foo");

            Assert.AreEqual(fieldName1, fieldName2);
        }

        [TestMethod]
        public void Should_Not_Be_Equal()
        {
            var fieldName1 = FieldName.Create("foo");
            var fieldName2 = FieldName.Create("bar");

            Assert.AreNotEqual(fieldName1, fieldName2);
        }

        [TestMethod]
        public void Should_Have_Same_HashCode()
        {
            var fieldName1 = FieldName.Create("foo");
            var fieldName2 = FieldName.Create("foo");

            Assert.AreEqual(fieldName1.GetHashCode(), fieldName2.GetHashCode());
        }

        [TestMethod]
        public void Should_Not_Have_Same_HashCode()
        {
            var fieldName1 = FieldName.Create("foo");
            var fieldName2 = FieldName.Create("bar");

            Assert.AreNotEqual(fieldName1.GetHashCode(), fieldName2.GetHashCode());
        }


        [TestMethod]
        public void Should_Not_Be_Equal_With_Operator()
        {
            var fieldName1 = FieldName.Create("foo");
            var fieldName2 = FieldName.Create("bar");

            Assert.IsTrue(fieldName1 != fieldName2);
        }

        [TestMethod]
        public void Should_Be_Equal_With_Null()
        {
            var fieldName1 = FieldName.Create("foo");
            FieldName? fieldName2 = null;

            Assert.IsFalse(fieldName1 == fieldName2);
        }

        [TestMethod]
        public void Should_Not_Be_Equal_With_Null()
        {
            var fieldName1 = FieldName.Create("foo");
            FieldName? fieldName2 = null;

            Assert.IsTrue(fieldName1 != fieldName2);
        }

        [TestMethod]
        public void Should_Be_Equal_With_Null_With_Operator()
        {
            var fieldName1 = FieldName.Create("foo");
            FieldName? fieldName2 = null;

            Assert.IsFalse(fieldName1.Equals(fieldName2));
        }

        [TestMethod]
        public void Should_Has_BrokenRules()
        {
            var vo = FieldRequired.Create("foo");

            vo.Value = "";

            Assert.IsFalse(vo.IsValid);
        }

        [TestMethod]
        public void Should_Not_Has_BrokenRules()
        {
            var vo = FieldRequired.Create("foo");

            vo.Value = "bar";

            Assert.IsTrue(vo.IsValid);
        }

        [TestMethod]
        public void Should_Has_BrokenRules_In_Create_Mode()
        {
            var vo = FieldRequired.Create("");

            Assert.IsFalse(vo.IsValid);
        }

        [TestMethod]
        public void Should_Not_Has_BrokenRules_In_Create_Mode()
        {
            var vo = FieldRequired.Create("foo");

            Assert.IsTrue(vo.IsValid);
        }

        [TestMethod]
        public void Should_Has_BrokenRules_In_Create_Mode_With_Validation()
        {
            var vo = FieldRequired.Create("");

            Assert.IsFalse(vo.IsValid);
        }

        [TestMethod]
        public void Should_Not_Has_BrokenRules_In_Create_Mode_With_Validation()
        {
            var vo = FieldRequired.Create("foo");

            Assert.IsTrue(vo.IsValid);
        }

        [TestMethod]
        public void Should_Has_BrokenRules_In_Create_Mode_With_Validation_With_Validator()
        {
            var vo = FieldRequired.Create("");

            Assert.IsFalse(vo.IsValid);
        }

        [TestMethod]
        public void Should_Not_Has_BrokenRules_In_Create_Mode_With_Validation_With_Validator()
        {
            var vo = FieldRequired.Create("foo");

            Assert.IsTrue(vo.IsValid);
        }

        [TestMethod]
        public void Should_Add_Validator_Ok() {
            var vo = FieldRequired.Create("foo");

            vo.AddValidator(new StubValueObjectValidator(vo));

            Assert.IsTrue(vo.IsValid);
        }

        [TestMethod]
        public void Should_Remove_Validator_Ok()
        {
            var vo = FieldRequired.Create("foo");

            vo.AddValidator(new StubValueObjectValidator(vo));
            vo.RemoveValidator(new StubValueObjectValidator(vo));

            Assert.IsTrue(vo.IsValid);
        }

        [TestMethod]
        public void Should_Add_Validators_Ok()
        {
            var vo = FieldRequired.Create("foo");

            vo.AddValidators(new List<AbstractRuleValidator<ValueObject<string>>> { new StubValueObjectValidator(vo) });

            Assert.IsTrue(vo.IsValid);
        }

        [TestMethod]
        public void Should_Has_BrokenRules_With_Validator()
        {
            var vo = FieldRequired.Create("");

            vo.AddValidator(new StubValueObjectValidator(vo));

            Assert.IsFalse(vo.IsValid);
        }

        [TestMethod]
        public void Should_Not_Has_BrokenRules_With_Validator()
        {
            var vo = FieldRequired.Create("foo");

            vo.AddValidator(new StubValueObjectValidator(vo));

            Assert.IsTrue(vo.IsValid);
        }

        [TestMethod]
        public void Should_Has_BrokenRules_With_Validators()
        {
            var vo = FieldRequired.Create("");

            vo.AddValidators(new List<AbstractRuleValidator<ValueObject<string>>> { new StubValueObjectValidator(vo) });

            Assert.IsFalse(vo.IsValid);
        }

        [TestMethod]
        public void Should_Not_Has_BrokenRules_With_Validators()
        {
            var vo = FieldRequired.Create("foo");

            vo.AddValidators(new List<AbstractRuleValidator<ValueObject<string>>> { new StubValueObjectValidator(vo) });

            Assert.IsTrue(vo.IsValid);
        }

        [TestMethod]
        public void Should_Has_BrokenRules_With_Validator_With_Validation()
        {
            var vo = FieldRequired.Create("");

            vo.AddValidator(new StubValueObjectValidator(vo));

            Assert.IsFalse(vo.IsValid);
        }

        [TestMethod]
        public void Should_Not_Has_BrokenRules_With_Validator_With_Validation()
        {
            var vo = FieldRequired.Create("foo");

            vo.AddValidator(new StubValueObjectValidator(vo));

            Assert.IsTrue(vo.IsValid);
        }

        [TestMethod]
        public void Should_Has_BrokenRules_With_Validators_With_Validation()
        {
            var vo = FieldRequired.Create("");

            vo.AddValidators(new List<AbstractRuleValidator<ValueObject<string>>> { new StubValueObjectValidator(vo) });

            Assert.IsFalse(vo.IsValid);
        }

        [TestMethod]
        public void Should_Not_Has_BrokenRules_With_Validators_With_Validation()
        {
            var vo = FieldRequired.Create("foo");

            vo.AddValidators(new List<AbstractRuleValidator<ValueObject<string>>> { new StubValueObjectValidator(vo) });

            Assert.IsTrue(vo.IsValid);
        }

        [TestMethod]
        public void Should_Has_BrokenRules_With_Validator_With_Validation_With_Validator()
        {
            var vo = FieldRequired.Create("");

            vo.AddValidator(new StubValueObjectValidator(vo));

            Assert.IsFalse(vo.IsValid);
        }

        [TestMethod]
        public void Should_Not_Has_BrokenRules_With_Validator_With_Validation_With_Validator()
        {
            var vo = FieldRequired.Create("foo");

            vo.AddValidator(new StubValueObjectValidator(vo));

            Assert.IsTrue(vo.IsValid);
        }
    }
}

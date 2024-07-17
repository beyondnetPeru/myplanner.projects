using BeyondNet.Ddd.Extensions;
using BeyondNet.Ddd.Rules;
using BeyondNet.Ddd.Rules.Impl;
using BeyondNet.Ddd.Test.Stubs;
using BeyondNet.Ddd.ValueObjects;
using Moq;
using Shouldly;

namespace BeyondNet.Ddd.Test
{
    [TestClass]
    public class EntityTest
    {
        [TestMethod]
        public void Should_Implement_Entity()
        {
            var owner = ParentRootEntity.Create(StringValueObject.Create("foo"), FieldName.Create("bar"));

            Assert.IsInstanceOfType(owner, typeof(Entity<ParentRootEntity>));
        }

        [TestMethod]
        public void Should_Have_Empty_DomainEvents_Collection()
        {
            var owner = ParentRootEntity.Create(StringValueObject.Create("foo"), FieldName.Create("foo"));

            Assert.AreEqual(0, owner.DomainEvents.Count);
        }

        [TestMethod]
        public void Should_Get_PropertiesBrokenRules()
        {
            var owner = ParentRootEntity.Create(StringValueObject.Create("foo"), FieldName.Create(""));

            var properties = owner.GetType().GetProperties();

            var result = properties.GetPropertiesBrokenRules(owner);

            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public void Should_Validate_Entity_NotValid()
        {
            var owner = ParentRootEntity.Create(StringValueObject.Create("foo"), FieldName.Create(""));

            owner.Validate();

            owner.IsValid.ShouldBeFalse();  
        }

        [TestMethod]
        public void Should_Validate_Entity_Valid()
        {
            var owner = ParentRootEntity.Create(StringValueObject.Create("foo"), FieldName.Create("bar"));

            owner.Validate();

            owner.IsValid.ShouldBeTrue();
        }

        [TestMethod]
        public void Should_Add_BrokenRule()
        {
            var owner = ParentRootEntity.Create(StringValueObject.Create("foo"), FieldName.Create(""));

            owner.AddBrokenRule("FieldName", "Field Name is required");

            Assert.AreEqual(1, owner._brokenRules.GetBrokenRules().Count);
        }

        [TestMethod]
        public void Should_Add_DomainEvent()
        {
            var owner = ParentRootEntity.Create(StringValueObject.Create("foo"), FieldName.Create(""));

            owner.AddDomainEvent(new StubMockDomainEvent());

            Assert.AreEqual(1, owner.DomainEvents.Count);
        }

        [TestMethod]
        public void Should_Remove_DomainEvent()
        {
            var owner = ParentRootEntity.Create(StringValueObject.Create("foo"), FieldName.Create(""));

            var domainEvent = new StubMockDomainEvent();

            owner.AddDomainEvent(domainEvent);

            owner.RemoveDomainEvent(domainEvent);

            Assert.AreEqual(0, owner.DomainEvents.Count);
        }

        [TestMethod]
        public void Should_Clear_DomainEvents()
        {
            var owner = ParentRootEntity.Create(StringValueObject.Create("foo"), FieldName.Create(""));

            owner.AddDomainEvent(new StubMockDomainEvent());

            owner.ClearDomainEvents();

            Assert.AreEqual(0, owner.DomainEvents.Count);
        }

        [TestMethod]
        public void Should_Set_Version()
        {
            var owner = ParentRootEntity.Create(StringValueObject.Create("foo"), FieldName.Create(""));

            owner.SetVersion(1);

            Assert.AreEqual(1, owner.Version);
        }

        [TestMethod]
        public void Should_Add_Validator()
        {
            var owner = ParentRootEntity.Create(StringValueObject.Create("foo"), FieldName.Create(""));

            owner.AddValidator(new StubEntityRuleValidator<ParentRootEntity>(owner));

            owner._validatorRules.GetValidators().Count.ShouldBeGreaterThan(0);
        }

        [TestMethod]
        public void Should_Validate_Entity_With_Validator()
        {
            var owner = ParentRootEntity.Create(StringValueObject.Create("foo"), FieldName.Create(""));

            owner.AddValidator(new StubEntityRuleValidator<ParentRootEntity>(owner));

            owner.Validate();

            owner.IsValid.ShouldBeFalse();
        }

        [TestMethod]
        public void Should_Validate_Entity_With_MockValidator()
        {
            var owner = ParentRootEntity.Create(StringValueObject.Create("foo"), FieldName.Create(""));

            var mockValidator = new Mock<StubEntityRuleValidator<ParentRootEntity>>(owner);

            mockValidator.Setup(x => x.AddRules(It.IsAny<RuleContext>()));

            owner.AddValidator(mockValidator.Object);

            owner.Validate();

            mockValidator.Verify(x => x.AddRules(It.IsAny<RuleContext>()), Times.Once);
        }

        [TestMethod]
        public void Should_Validate_Entity_With_MockValidator_NotValid()
        {
            var owner = ParentRootEntity.Create(StringValueObject.Create("foo"), FieldName.Create(""));

            var mockValidator = new Mock<StubEntityRuleValidator<ParentRootEntity>>(owner);

            mockValidator.Setup(x => x.AddRules(It.IsAny<RuleContext>())).Callback(() => owner.AddBrokenRule("FieldName", "Field Name is required"));

            owner.AddValidator(mockValidator.Object);

            owner.Validate();

            owner.IsValid.ShouldBeFalse();
        }

        [TestMethod]
        public void Should_Validate_Entity_With_MockValidator_Valid()
        {
            var owner = ParentRootEntity.Create(StringValueObject.Create("foo"), FieldName.Create("bar"));

            var mockValidator = new Mock<StubEntityRuleValidator<ParentRootEntity>>(owner);

            mockValidator.Setup(x => x.AddRules(It.IsAny<RuleContext>()));

            owner.AddValidator(mockValidator.Object);

            owner.Validate();

            owner.IsValid.ShouldBeTrue();
        }

        [TestMethod]
        public void Should_Validate_Entity_With_MockValidator_With_BrokenRule()
        {
            var owner = ParentRootEntity.Create(StringValueObject.Create("foo"), FieldName.Create(""));

            var mockValidator = new Mock<StubEntityRuleValidator<ParentRootEntity>>(owner);

            mockValidator.Setup(x => x.AddRules(It.IsAny<RuleContext>())).Callback(() => owner.AddBrokenRule("FieldName", "Field Name is required"));

            owner.AddValidator(mockValidator.Object);

            owner.Validate();

            owner._brokenRules.GetBrokenRules().Count.ShouldBeGreaterThan(0);
        }

        [TestMethod]
        public void Should_Validate_Entity_With_MockValidator_With_BrokenRule_Message()
        {
            var owner = ParentRootEntity.Create(StringValueObject.Create("foo"), FieldName.Create(""));

            var mockValidator = new Mock<StubEntityRuleValidator<ParentRootEntity>>(owner);

            mockValidator.Setup(x => x.AddRules(It.IsAny<RuleContext>())).Callback(() => owner.AddBrokenRule("FieldName", "Field Name is required"));

            owner.AddValidator(mockValidator.Object);

            owner.Validate();

            owner._brokenRules.GetBrokenRules()[0].Message.ShouldBe("Field Name is required");
        }

        [TestMethod]
        public void Should_Validate_Entity_With_MockValidator_With_BrokenRule_PropertyName()
        {
            var owner = ParentRootEntity.Create(StringValueObject.Create("foo"), FieldName.Create(""));

            var mockValidator = new Mock<StubEntityRuleValidator<ParentRootEntity>>(owner);

            mockValidator.Setup(x => x.AddRules(It.IsAny<RuleContext>())).Callback(() => owner.AddBrokenRule("FieldName", "Field Name is required"));

            owner.AddValidator(mockValidator.Object);

            owner.Validate();

            owner._brokenRules.GetBrokenRules()[0].Property.ShouldBe("FieldName");
        }

        [TestMethod]
        public void Should_Validate_Entity_With_MockValidator_With_BrokenRule_PropertyName_NotValid()
        {
            var owner = ParentRootEntity.Create(StringValueObject.Create("foo"), FieldName.Create(""));

            var mockValidator = new Mock<StubEntityRuleValidator<ParentRootEntity>>(owner);

            mockValidator.Setup(x => x.AddRules(It.IsAny<RuleContext>())).Callback(() => owner.AddBrokenRule("FieldName", "Field Name is required"));

            owner.AddValidator(mockValidator.Object);

            owner.Validate();

            owner._brokenRules.GetBrokenRules()[0].Property.ShouldBe("FieldName");
        }

        [TestMethod]
        public void Should_Validate_Entity_With_MockValidator_With_BrokenRule_PropertyName_Valid()
        {
            var owner = ParentRootEntity.Create(StringValueObject.Create("foo"), FieldName.Create("bar"));

            var mockValidator = new Mock<StubEntityRuleValidator<ParentRootEntity>>(owner);

            mockValidator.Setup(x => x.AddRules(It.IsAny<RuleContext>()));

            owner.AddValidator(mockValidator.Object);

            owner.Validate();

            owner._brokenRules.GetBrokenRules().Count.ShouldBe(0);
        }

        [TestMethod]
        public void Should_Validate_Entity_With_MockValidator_With_BrokenRule_Message_Valid()
        {
            var owner = ParentRootEntity.Create(StringValueObject.Create("foo"), FieldName.Create("bar"));

            var mockValidator = new Mock<StubEntityRuleValidator<ParentRootEntity>>(owner);

            mockValidator.Setup(x => x.AddRules(It.IsAny<RuleContext>()));

            owner.AddValidator(mockValidator.Object);

            owner.Validate();

            owner._brokenRules.GetBrokenRules().Count.ShouldBe(0);
        }

        [TestMethod]
        public void Should_Validate_Entity_With_MockValidator_With_BrokenRule_Message_NotValid()
        {
            var owner = ParentRootEntity.Create(StringValueObject.Create("foo"), FieldName.Create(""));

            var mockValidator = new Mock<StubEntityRuleValidator<ParentRootEntity>>(owner);

            mockValidator.Setup(x => x.AddRules(It.IsAny<RuleContext>())).Callback(() => owner.AddBrokenRule("FieldName", "Field Name is required"));

            owner.AddValidator(mockValidator.Object);

            owner.Validate();

            owner._brokenRules.GetBrokenRules().Count.ShouldBeGreaterThan(0);
        }

        [TestMethod]
        public void Should_Validate_Entity_With_MockValidator_With_BrokenRule_Message_Valid_With_MockValidator()
        {
            var owner = ParentRootEntity.Create(StringValueObject.Create("foo"), FieldName.Create("bar"));

            var mockValidator = new Mock<StubEntityRuleValidator<ParentRootEntity>>(owner);

            mockValidator.Setup(x => x.AddRules(It.IsAny<RuleContext>()));

            owner.AddValidator(mockValidator.Object);

            owner.Validate();

            owner._brokenRules.GetBrokenRules().Count.ShouldBe(0);
        }

        [TestMethod]
        public void Should_Validate_Entity_With_MockValidator_With_BrokenRule_Message_NotValid_With_MockValidator()
        {
            var owner = ParentRootEntity.Create(StringValueObject.Create("foo"), FieldName.Create(""));

            var mockValidator = new Mock<StubEntityRuleValidator<ParentRootEntity>>(owner);

            mockValidator.Setup(x => x.AddRules(It.IsAny<RuleContext>())).Callback(() => owner.AddBrokenRule("FieldName", "Field Name is required"));

            owner.AddValidator(mockValidator.Object);

            owner.Validate();

            owner._brokenRules.GetBrokenRules().Count.ShouldBeGreaterThan(0);
        }

        [TestMethod]
        public void Should_Validate_Entity_With_MockValidator_With_BrokenRule_Message_Valid_With_MockValidator_With_BrokenRule()
        {
            var owner = ParentRootEntity.Create(StringValueObject.Create("foo"), FieldName.Create("bar"));

            var mockValidator = new Mock<StubEntityRuleValidator<ParentRootEntity>>(owner);

            mockValidator.Setup(x => x.AddRules(It.IsAny<RuleContext>()));

            owner.AddValidator(mockValidator.Object);

            owner.Validate();

            owner._brokenRules.GetBrokenRules().Count.ShouldBe(0);
        }

        [TestMethod]
        public void Should_Validate_Entity_With_MockValidator_With_BrokenRule_Message_NotValid_With_MockValidator_With_BrokenRule()
        {
            var owner = ParentRootEntity.Create(StringValueObject.Create("foo"), FieldName.Create(""));

            var mockValidator = new Mock<StubEntityRuleValidator<ParentRootEntity>>(owner);

            mockValidator.Setup(x => x.AddRules(It.IsAny<RuleContext>())).Callback(() => owner.AddBrokenRule("FieldName", "Field Name is required"));

            owner.AddValidator(mockValidator.Object);

            owner.Validate();

            owner._brokenRules.GetBrokenRules().Count.ShouldBeGreaterThan(0);
        }

        [TestMethod]
        public void Should_Validate_Entity_With_MockValidator_With_BrokenRule_Message_Valid_With_MockValidator_With_BrokenRule_Message()
        {
            var owner = ParentRootEntity.Create(StringValueObject.Create("foo"), FieldName.Create("bar"));

            var mockValidator = new Mock<StubEntityRuleValidator<ParentRootEntity>>(owner);

            mockValidator.Setup(x => x.AddRules(It.IsAny<RuleContext>()));

            owner.AddValidator(mockValidator.Object);

            owner.Validate();

            owner._brokenRules.GetBrokenRules().Count.ShouldBe(0);
        }

        [TestMethod]
        public void Should_Validate_Entity_With_MockValidator_With_BrokenRule_Message_NotValid_With_MockValidator_With_BrokenRule_Message()
        {
            var owner = ParentRootEntity.Create(StringValueObject.Create("foo"), FieldName.Create(""));

            var mockValidator = new Mock<StubEntityRuleValidator<ParentRootEntity>>(owner);

            mockValidator.Setup(x => x.AddRules(It.IsAny<RuleContext>())).Callback(() => owner.AddBrokenRule("FieldName", "Field Name is required"));

            owner.AddValidator(mockValidator.Object);

            owner.Validate();

            owner._brokenRules.GetBrokenRules().Count.ShouldBeGreaterThan(0);
        }

        [TestMethod]
        public void Should_Validate_Entity_With_MockValidator_With_BrokenRule_Message_Valid_With_MockValidator_With_BrokenRule_Message_NotValid()
        {
            var owner = ParentRootEntity.Create(StringValueObject.Create("foo"), FieldName.Create("bar"));

            var mockValidator = new Mock<StubEntityRuleValidator<ParentRootEntity>>(owner);

            mockValidator.Setup(x => x.AddRules(It.IsAny<RuleContext>()));

            owner.AddValidator(mockValidator.Object);

            owner.Validate();

            owner._brokenRules.GetBrokenRules().Count.ShouldBe(0);
        }

        [TestMethod]
        public void Should_Validate_Entity_With_MockValidator_With_BrokenRule_Message_NotValid_With_MockValidator_With_BrokenRule_Message_NotValid()
        {
            var owner = ParentRootEntity.Create(StringValueObject.Create("foo"), FieldName.Create(""));

            var mockValidator = new Mock<StubEntityRuleValidator<ParentRootEntity>>(owner);

            mockValidator.Setup(x => x.AddRules(It.IsAny<RuleContext>())).Callback(() => owner.AddBrokenRule("FieldName", "Field Name is required"));

            owner.AddValidator(mockValidator.Object);

            owner.Validate();

            owner._brokenRules.GetBrokenRules().Count.ShouldBeGreaterThan(0);
        }

        [TestMethod]
        public void Should_Validate_Entity_With_MockValidator_With_BrokenRule_Message_Valid_With_MockValidator_With_BrokenRule_Message_Valid()
        {
            var owner = ParentRootEntity.Create(StringValueObject.Create("foo"), FieldName.Create("bar"));

            var mockValidator = new Mock<StubEntityRuleValidator<ParentRootEntity>>(owner);

            mockValidator.Setup(x => x.AddRules(It.IsAny<RuleContext>()));

            owner.AddValidator(mockValidator.Object);

            owner.Validate();

            owner._brokenRules.GetBrokenRules().Count.ShouldBe(0);
        }

        [TestMethod]
        public void Should_Validate_Entity_With_MockValidator_With_BrokenRule_Message_NotValid_With_MockValidator_With_BrokenRule_Message_Valid()
        {
            var owner = ParentRootEntity.Create(StringValueObject.Create("foo"), FieldName.Create(""));

            var mockValidator = new Mock<StubEntityRuleValidator<ParentRootEntity>>(owner);

            mockValidator.Setup(x => x.AddRules(It.IsAny<RuleContext>())).Callback(() => owner.AddBrokenRule("FieldName", "Field Name is required"));

            owner.AddValidator(mockValidator.Object);

            owner.Validate();

            owner._brokenRules.GetBrokenRules().Count.ShouldBeGreaterThan(0);
        }
    }
}

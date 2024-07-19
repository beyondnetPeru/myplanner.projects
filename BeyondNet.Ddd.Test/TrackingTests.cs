using Shouldly;
using BeyondNet.Ddd.Test.Stubs;
using BeyondNet.Ddd.ValueObjects;

namespace BeyondNet.Ddd.Test
{
    [TestClass]
    public class TrackingTests
    {
        [TestMethod]
        public void MarkNew_ShouldSetIsNewToTrue_AndIsDirtyToFalse()
        {
            var name = StringValueObject.Create("foo");
            var fieldName = FieldName.Create("foo");

            var subject = ParentRootEntity.Create(name, fieldName);

            subject.Tracking.IsNew.ShouldBeTrue();
            subject.Tracking.IsDirty.ShouldBeFalse();
        }

        [TestMethod]
        public void MarkNew_ShouldSetIsNewTofalse_AndIsDirtyToTrue()
        {
            var name = StringValueObject.Create("foo");
            var fieldName = FieldName.Create("foo");

            var subject = ParentRootEntity.Create(name, fieldName);

            var props = subject.GetProps();
            
            props.FieldName!.SetValue("bar");

            subject.SetProps(props);

            subject.Tracking.IsNew.ShouldBeFalse();
            subject.Tracking.IsDirty.ShouldBeTrue();

        }
    }
}

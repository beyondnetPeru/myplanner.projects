using Ddd.Interfaces;
using Ddd.ValueObjects;

namespace BeyondNet.Ddd.Test.Stubs
{
    public class ParentRootEntity : Entity<ParentRootEntity>, IAggregateRoot
    {

        public FieldName FieldName { get; }
        public StringValueObject Name { get; }

        private ParentRootEntity(StringValueObject name, FieldName fieldName)
        {
            Name = name;
            FieldName = fieldName;
        }

        public static ParentRootEntity Create(StringValueObject name, FieldName fieldName)
        {
            return new ParentRootEntity(name, fieldName);
        }

    }
}

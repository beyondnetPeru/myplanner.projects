using BeyondNet.Ddd.Interfaces;
using BeyondNet.Ddd.ValueObjects;

namespace BeyondNet.Ddd.Test.Stubs
{
    public class ParentRootProps: IProps
    {
        public FieldName? FieldName { get; set; }
        public StringValueObject? Name { get; set; }

        public object Clone()
        {
            return new ParentRootProps
            {
                FieldName = FieldName,
                Name = Name
            };
        }
    }

    public class ParentRootEntity : Entity<ParentRootEntity, ParentRootProps>, IAggregateRoot
    {
        public ParentRootEntity(ParentRootProps props) : base(props)
        {
        }

        public static ParentRootEntity Create(StringValueObject name, FieldName fieldName)
        {
            var props = new ParentRootProps
            {
                FieldName = fieldName,
                Name = name
            };

            return new ParentRootEntity(props);
        }

    }
}

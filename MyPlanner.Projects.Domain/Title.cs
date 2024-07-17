using BeyondNet.Ddd.ValueObjects;

namespace MyPlanner.Projects.Domain
{
    public class Title : StringValueObject
    {
        public Title(string value) : base(value)
        {

        }

        public static Title Create(string value)
        {
            return new Title(value);
        }

    }
}

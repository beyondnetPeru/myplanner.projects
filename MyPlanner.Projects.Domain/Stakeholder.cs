using BeyondNet.Ddd;


namespace MyPlanner.Projects.Domain
{
    public struct StakeHolderProp
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Rol { get; set; }

        public string Email { get; set; }

    }

    public class Stakeholder : ValueObject<StakeHolderProp>
    {
        private Stakeholder(StakeHolderProp value) : base(value)
        {
        }

        public static Stakeholder Create(StakeHolderProp value)
        {
            return new Stakeholder(value);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value.Name;
            yield return Value.Description;
            yield return Value.Rol;
            yield return Value.Email;
        }
    }
}

namespace BeyondNet.Ddd.Test.Stubs
{
    public class MockEnumeration : Enumeration
    {
        public static MockEnumeration First = new MockEnumeration(1, "First");
        public static MockEnumeration Second = new MockEnumeration(2, "Second");

        public MockEnumeration(int id, string name) : base(id, name)
        {
        }
    }
}
using BeyondNet.Ddd;
using BeyondNet.Ddd.ValueObjects;

namespace MyProjects.Domain.ReleaseAggregate
{
    public class ReleaseComment : Entity<ReleaseComment>
    {
        public StringValueObject Text { get; set; }
        public DateTime Date { get; set; }

        private ReleaseComment(StringValueObject text, DateTime date)
        {
            Text = text;
            Date = date;
        }


        public static ReleaseComment Create(StringValueObject text, DateTime date)
        {
            return new ReleaseComment(text, date);
        }

        public void Update(StringValueObject text, DateTime date)
        {
            Text = text;
            Date = date;
        }
    }
}

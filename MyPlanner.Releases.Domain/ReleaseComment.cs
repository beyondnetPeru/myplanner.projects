using BeyondNet.Ddd;
using BeyondNet.Ddd.Interfaces;
using BeyondNet.Ddd.ValueObjects;

namespace MyProjects.Domain.ReleaseAggregate
{
    public class ReleaseCommentProps : IProps
    {
        public StringValueObject Text { get; set; }
        public DateTime Date { get; set; }

        public ReleaseCommentProps(StringValueObject text)
        {
            Text = text;
            Date = DateTime.Now;
        }

        public object Clone()
        {
            return new ReleaseCommentProps(Text)
            {
                Date = Date
            };

        }
    }

    public class ReleaseComment : Entity<ReleaseComment, ReleaseCommentProps>
    {

        private ReleaseComment(ReleaseCommentProps props) : base(props)
        {

        }

        public static ReleaseComment Create(StringValueObject text)
        {
            var props = new ReleaseCommentProps(text);

            return new ReleaseComment(props);
        }

        public void Update(StringValueObject text, DateTime date)
        {
            var props = GetProps();
            
            props.Text = text;
            props.Date = date;

            SetProps(props);
        }
    }
}

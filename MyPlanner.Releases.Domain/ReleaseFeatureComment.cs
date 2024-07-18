using BeyondNet.Ddd;
using BeyondNet.Ddd.Interfaces;
using BeyondNet.Ddd.ValueObjects;
using MyProjects.Domain.ReleaseAggregate;

namespace MyPlanner.Releases.Domain
{
    public class ReleaseFeatureCommentProps : IProps
    {

        public StringValueObject Comment { get; set; }
        public StringValueObject Author { get; set; }
        public StringValueObject Date { get; set; }

        public ReleaseFeatureCommentProps(StringValueObject comment, StringValueObject author, StringValueObject date)
        {
            Comment = comment;
            Author = author;
            Date = date;
        }

        public object Clone()
        {
            return new ReleaseFeatureCommentProps(Comment, Author, Date);
        }
    }
    public class ReleaseFeatureComment : Entity<ReleaseFeatureComment, ReleaseFeatureCommentProps>
    {

        private ReleaseFeatureComment(ReleaseFeatureCommentProps props) : base(props)
        {
        }

        public static ReleaseFeatureComment Create(StringValueObject comment, StringValueObject author, StringValueObject date)
        {
            var props = new ReleaseFeatureCommentProps(comment, author, date);

            return new ReleaseFeatureComment(props);
        }

        public void UpdateComment(StringValueObject comment)
        {
            Props.Comment = comment;
        }

        public void UpdateAuthor(StringValueObject author)
        {
            Props.Author = author;
        }

        public void UpdateDate(StringValueObject date)
        {
            Props.Date = date;
        }
    }
}

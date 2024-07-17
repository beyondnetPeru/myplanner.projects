using BeyondNet.Ddd;
using BeyondNet.Ddd.ValueObjects;

namespace MyPlanner.Releases.Domain
{
    public  class ReleaseFeatureComment : Entity<ReleaseFeatureComment>
    {
        public StringValueObject Comment { get; set; } 
        public StringValueObject Author { get; set; }
        public StringValueObject Date { get; set; }

        private ReleaseFeatureComment(StringValueObject comment, StringValueObject author, StringValueObject date)
        {
            Comment = comment;
            Author = author;
            Date = date;
        }

        public static ReleaseFeatureComment Create(StringValueObject comment, StringValueObject author, StringValueObject date)
        {
            return new ReleaseFeatureComment(comment, author, date);
        }

        public void UpdateComment(StringValueObject comment)
        {
            Comment = comment;
        }

        public void UpdateAuthor(StringValueObject author)
        {
            Author = author;
        }

        public void UpdateDate(StringValueObject date)
        {
            Date = date;
        }
    }
}

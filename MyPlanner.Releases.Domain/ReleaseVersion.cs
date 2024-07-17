using BeyondNet.Ddd;

namespace MyPlanner.Releases.Domain
{
    public enum StageEnum
    {
        Alpha = 1,
        Beta = 2,
        ReleaseCandidate = 3,
        Release = 4,
        GeneralAvailability = 5,
        PostRelease = 6
    }

    public class ReleaseVersionProps
    {
        public StageEnum Stage { get; set; }
        public int Major { get; set; }
        public int Minor { get; set; }
        public int Patch { get; set; }

        public ReleaseVersionProps(StageEnum stage, int major, int minor, int patch)
        {
            Stage = stage;
            Major = major;
            Minor = minor;
            Patch = patch;
        }
    }

    public class ReleaseVersion : ValueObject<ReleaseVersionProps>
    {
        private ReleaseVersion(ReleaseVersionProps value) : base(value)
        {
        }

        public StageEnum Stage { get; } 
        public int Major { get; }
        public int Minor { get; }
        public int Patch { get; }

     
        public static ReleaseVersion Create(StageEnum stage, int major, int minor, int patch)
        {
            return new ReleaseVersion(new ReleaseVersionProps(stage, major, minor, patch));
        }

        public string GetFullVersion()
        {
            return $"{Stage} {Major}.{Minor}.{Patch}";
        }

        public string GetVersion()
        {
            return $"{Major}.{Minor}.{Patch}";
        }


        protected override IEnumerable<object> GetEqualityComponents()
        {
            throw new NotImplementedException();
        }
    }
}

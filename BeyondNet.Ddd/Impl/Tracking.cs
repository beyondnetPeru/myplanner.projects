namespace BeyondNet.Ddd.Impl
{
    public class Tracking
    {
        public bool IsNew { get; private set; }
        public bool IsDirty { get; private set; }

        public Tracking MarkNew()
        {
            IsNew = true;
            IsDirty = false;

            return this;
        }

        public Tracking MarkDefault()
        {
            IsNew = false;
            IsDirty = false;

            return this;
        }

        public Tracking MarkDirty()
        {
            IsNew = false;
            IsDirty = true;

            return this;
        }
    }
}

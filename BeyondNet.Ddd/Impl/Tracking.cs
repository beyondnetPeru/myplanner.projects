using BeyondNet.Ddd.Interfaces;

namespace BeyondNet.Ddd.Impl
{
    public class Tracking
    {
        public bool IsNew { get; private set; }
        public bool IsDirty { get; private set; }

        public static Tracking MarkClean()
        {
            return new Tracking() { 
                IsNew = false,
                IsDirty = false
            };
        }

        public static Tracking MarkDirty<TProp>(TProp props) where TProp : IProps
        {
            var isDirty = FindChanges(props);

            if (isDirty)
            {
                return new Tracking()
                {
                    IsDirty = true,
                    IsNew = false
                };
            }

            return Tracking.MarkNew();
        }

        public static Tracking MarkDirty() 
        {
            return new Tracking()
            {
                IsDirty = true,
                IsNew = false
            };
        }

        public static Tracking MarkNew()
        {
            return new Tracking
            {
                IsDirty = false,
                IsNew = true
            };
        }

        protected static bool FindChanges<TProp>(TProp props) where TProp : IProps
        {
            foreach (var prop in props.GetType().GetProperties())
            {
                object value = prop.GetValue(props)!;

                var trackingProperty = value.GetType().GetProperty("Tracking");

                if (trackingProperty != null)
                {
                    var trackingValue = (Tracking)trackingProperty.GetValue(value)!;

                    if (trackingValue.IsDirty)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}

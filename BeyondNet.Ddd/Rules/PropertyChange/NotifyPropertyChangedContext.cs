namespace BeyondNet.Ddd.Rules.PropertyChange
{
    public class NotifyPropertyChangedContext
    {
        internal object Value { get; set; }
        internal Type Type { get; }

        internal event NotifyPropertyChangedHandler PropertyChangedCallback;

        internal NotifyPropertyChangedContext(object defaultValue, Type type, NotifyPropertyChangedHandler notifyPropertyChangedHandler)
        {
            Value = defaultValue;
            Type = type;

            PropertyChangedCallback += notifyPropertyChangedHandler;
        }

        internal void InvokePropertyChangedCallback(NotifyPropertyChangedBase sender, NotifyPropertyChangedContextArgs e)
        {
            PropertyChangedCallback?.Invoke(sender, e);
        }
    }
}

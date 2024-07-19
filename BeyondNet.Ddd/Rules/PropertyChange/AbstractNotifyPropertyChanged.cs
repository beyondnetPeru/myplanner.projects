using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BeyondNet.Ddd.Rules.PropertyChange
{
    public delegate void NotifyPropertyChangedHandler(AbstractNotifyPropertyChanged sender, NotifyPropertyChangedContextArgs e);

    public class AbstractNotifyPropertyChanged : INotifyPropertyChanged
    {
        #region Members

        public event PropertyChangedEventHandler? PropertyChanged;

        private readonly Dictionary<string, NotifyPropertyChangedContext> properties = new Dictionary<string, NotifyPropertyChangedContext>();

        protected bool IsEventInvokingEnabled { get; set; }

        protected bool IsCallbackInvokingEnabled { get; set; }

        #endregion

        #region Constructors

        protected AbstractNotifyPropertyChanged()
        {
            IsCallbackInvokingEnabled = true;
            IsEventInvokingEnabled = true;
        }

        #endregion

        #region Methods

        protected void RegisterProperty(string name, Type type, object defaultValue)
        {
            RegisterProperty(name, type, defaultValue, null);
        }

        protected void RegisterProperty(string name, Type type, object defaultValue, NotifyPropertyChangedHandler notifyPropertyChangedHandler)
        {
            ArgumentNullException.ThrowIfNull(name, nameof(name));
            ArgumentNullException.ThrowIfNull(type, nameof(type));

            ValidateValueForType(defaultValue, type);

            if (properties.ContainsKey(name))
            {
                throw new ArgumentException($"This class already contains registered property named '{name}'.");
            }

            properties.Add(name, new NotifyPropertyChangedContext(defaultValue, type, notifyPropertyChangedHandler));
        }

        public void RegisterPropertyChangedCallback(string propertyName, NotifyPropertyChangedHandler notifyPropertyChangedHandler)
        {
            ArgumentNullException.ThrowIfNull(propertyName, nameof(propertyName));
            ArgumentNullException.ThrowIfNull(notifyPropertyChangedHandler, nameof(notifyPropertyChangedHandler));

            GetPropertyContext(propertyName, nameof(propertyName)).PropertyChangedCallback += notifyPropertyChangedHandler;
        }

        protected void UnregisterPropertyChangedCallback(string propertyName, NotifyPropertyChangedHandler notifyPropertyChangedHandler)
        {
            ArgumentNullException.ThrowIfNull(propertyName, nameof(propertyName));

            GetPropertyContext(propertyName, nameof(propertyName)).PropertyChangedCallback -= notifyPropertyChangedHandler;
        }

        protected object GetValue([CallerMemberName] string propertyName = null)
        {
            return GetPropertyContext(propertyName, nameof(propertyName)).Value;
        }

        public void ForceSetValue(object value, [CallerMemberName] string propertyName = null)
        {
            SetValue(value, propertyName, true);
        }

        protected void SetValue(object value, [CallerMemberName] string propertyName = null)
        {
            SetValue(value, propertyName, false);
        }

        private void SetValue(object value, string propertyName, bool forceSetValue)
        {
            NotifyPropertyChangedContext propertyData = GetPropertyContext(propertyName, nameof(propertyName));

            ValidateValueForType(value, propertyData.Type);

            // Calling Equals calls the overriden method even when the value is boxed
            bool? valuesEqual = propertyData.Value?.Equals(value);

            if (forceSetValue || valuesEqual == null && !ReferenceEquals(value, null) || valuesEqual == false)
            {
                object oldValue = propertyData.Value!;

                propertyData.Value = value!;

                if (IsCallbackInvokingEnabled)
                {
                    propertyData.InvokePropertyChangedCallback(this, new NotifyPropertyChangedContextArgs(oldValue, value));
                }

                if (IsEventInvokingEnabled)
                {
                    OnNotifyPropertyChanged(propertyName);
                }
            }
        }

        protected virtual void OnNotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            ArgumentNullException.ThrowIfNull(propertyName, nameof(propertyName));

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        private NotifyPropertyChangedContext GetPropertyContext(string propertyName, string propertyNameParameterName)
        {
            ArgumentNullException.ThrowIfNull(propertyName, propertyNameParameterName);

            try
            {
                return properties[propertyName];
            }
            catch (KeyNotFoundException)
            {
                throw new ArgumentException($"There is no registered property called '{propertyName}'.", propertyNameParameterName);
            }
        }

        private void ValidateValueForType(object value, Type type)
        {
            if (value == null)
            {
                if (type.GetIsValueType() && Nullable.GetUnderlyingType(type) == null)
                {
                    throw new ArgumentException($"The type '{type}' is not a nullable type.");
                }
            }
            else
            {
                if (!type.GetIsAssignableFrom(value.GetType()))
                {
                    throw new ArgumentException($"The specified value cannot be assigned to a property of type ({type})");
                }
            }
        }

        #endregion
    }
}
